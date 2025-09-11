using System;
using System.Threading.Tasks;
using Astra;

namespace Crm.Accounts;

public interface IUserRepository : IAstraBasicWithPagedBasicRepository<User, Guid, UserPagedParameter>
{
    Task<User?> FindByNameAsync(string name, bool includeDetails = true);
    Task<User?> FindByEmailAsync(string email, bool includeDetails = true);

    /// <summary>
    /// 查找最后一个注册的用户
    /// </summary>
    /// <returns></returns>
    Task<User?> FindLastAsync(bool includeInternalUser = false);
}