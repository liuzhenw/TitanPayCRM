using AutoMapper;
using Crm.Accounts;

namespace Crm.Admin.Accounts;

public class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<UserCache, UserBasicDto>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserWithDetailsDto>()
            .ForMember(d => d.Roles,
                o => o.MapFrom(s => s.UserRoles.Select(x => x.RoleId)));
        CreateMap<UserQueryInput, UserPagedParameter>();
        
        CreateMap<RoleCache, RoleBasicDto>();
        CreateMap<Role, RoleDto>();
    }
}