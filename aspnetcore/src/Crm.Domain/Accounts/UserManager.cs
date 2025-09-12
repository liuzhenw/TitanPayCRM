using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Astra;
using Astra.Common;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Crm.Accounts;

public class UserManager(
    IUnitOfWorkManager uowManager,
    IUserRepository userRepo,
    IRoleRepository roleRepo) :
    DomainService
{
    public async Task<User> GetOrCreateAsync(string email)
    {
        var user = await userRepo.FindByEmailAsync(email);
        if (user is not null) return user;

        user = new User(GuidGenerator.Create(), email, email);
        await userRepo.InsertAsync(user, true);
        return user;
    }

    public async Task<User> ImportAsync(string email, DateTimeOffset createdAt)
    {
        var user = new User(GuidGenerator.Create(), email, email, createdAt);
        return await userRepo.InsertAsync(user);
    }

    public async Task<User> CreateAsync(string email, string name, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new UserFriendlyException("密码不能为空!");

        var user = await userRepo.FindByEmailAsync(email);
        if (user is not null)
            throw new UserFriendlyException("用户已存在!");

        user = new User(GuidGenerator.Create(), name, email);
        user.PasswordHash = CalculatePasswordHash(user, password);
        if (user.Name != AstraConsts.RootUser) return user;

        // 超级管理员添加 root 角色
        var root = await roleRepo.GetAsync(AstraConsts.RootRole);
        user.UserRoles.Add(new UserRole(GuidGenerator.Create(), user, root));
        return user;
    }

    public async Task<User?> FindByEmailOrNameAsync(string nameOrEmail)
    {
        return CheckHelper.IsEmailAddress(nameOrEmail)
            ? await userRepo.FindByEmailAsync(nameOrEmail)
            : await userRepo.FindByNameAsync(nameOrEmail);
    }

    public async Task ChangePassword(User user, string? oldPassword, string newPassword)
    {
        if (newPassword.IsNullOrWhiteSpace())
            throw new BusinessException(CrmErrorCodes.Accounts.InvalidPassword);

        if (oldPassword is not null) await PasswordAuthAsync(user, oldPassword);
        user.PasswordSalt = null;
        user.PasswordHash = CalculatePasswordHash(user, newPassword);
    }

    public async Task PasswordAuthAsync(User user, string password)
    {
        if (user.LockedAt > DateTimeOffset.Now)
            throw new BusinessException(CrmErrorCodes.Accounts.Locked);

        if(string.IsNullOrWhiteSpace(password))
            throw new BusinessException(CrmErrorCodes.Accounts.InvalidPassword);
        
        var passwordHash = CalculatePasswordHash(user, password);
        if (passwordHash != user.PasswordHash)
        {
            await OnAttemptFailed(user);
            throw new BusinessException(CrmErrorCodes.Accounts.InvalidPassword);
        }

        user.OnAttemptSucceeded();
        await userRepo.UpdateAsync(user);
    }

    public async Task OnAttemptFailed(User user)
    {
        using var uow = uowManager.Begin(true);
        user.OnAttemptFailed();
        await userRepo.UpdateAsync(user);
        await uow.CompleteAsync();
    }

    public async Task<List<Role>> GetRoleListAsync(User user)
    {
        await userRepo.EnsureCollectionLoadedAsync(user, x => x.UserRoles);
        if (user.UserRoles.Count < 1)
            return [];

        return await roleRepo.GetListByIdsAsync(user.UserRoles.Select(x => x.RoleId));
    }

    public async Task AddRolesAsync(User user, params IReadOnlyCollection<string> appendRoles)
    {
        await userRepo.EnsureCollectionLoadedAsync(user, x => x.UserRoles);
        var except = appendRoles.Except(user.UserRoles.Select(x => x.RoleId)).ToArray();
        if (except.Length < 1) return;

        var roles = await roleRepo.GetListByIdsAsync(except);
        if (roles.Count < 1) return;

        if (roles.Any(x => x is { IsPublic: false, IsStatic: true }))
            throw new UserFriendlyException("不允许添加非公开角色和静态角色!");

        var userRoles = roles.Select(x => new UserRole(GuidGenerator.Create(), user, x));
        user.UserRoles.AddRange(userRoles);
        user.UpdatedAt = DateTimeOffset.Now;
    }

    public async Task RemoveRolesAsync(User user, params IReadOnlyCollection<string> removeRoles)
    {
        var roles = await roleRepo.GetListByIdsAsync(removeRoles);
        if (roles.Any(x => x is { IsPublic: false, IsStatic: true }))
            throw new UserFriendlyException("不允许删除非公开角色和静态角色!");

        await userRepo.EnsureCollectionLoadedAsync(user, x => x.UserRoles);
        user.UserRoles.RemoveAll(x => removeRoles.Contains(x.RoleId));
        user.UpdatedAt = DateTimeOffset.Now;
    }

    private static string CalculatePasswordHash(User user, string password)
    {
        const int iterations = 100_000;
        user.PasswordSalt ??= Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var salt = Convert.FromBase64String(user.PasswordSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(hash);
    }
}

public record UserAuthResult(bool IsSuccess, string? ErrorCode)
{
    public static UserAuthResult Success() => new(true, null);
    public static UserAuthResult Fail(string errorCode) => new(false, errorCode);
}