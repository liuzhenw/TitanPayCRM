using Astra.AutoMapper.Converters;
using AutoMapper;
using Crm.Accounts;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.ObjectMapping;

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

public class UserValueConverter(IServiceProvider services) : 
    EntityCacheValueConverter<UserCache, UserBasicDto, Guid>(services);