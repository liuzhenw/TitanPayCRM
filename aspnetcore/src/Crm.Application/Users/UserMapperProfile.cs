using System;
using Astra.AutoMapper;
using Astra.AutoMapper.Converters;
using AutoMapper;
using Crm.Accounts;

namespace Crm.Users;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserCache, UserBasicDto>()
            .ToUrl(d => d.AvatarUrl, s => s.AvatarUri);
        
        CreateMap<User, UserDto>()
            .ForMember(d => d.HasPassword, o => o.MapFrom(s => s.PasswordHash != null))
            .ToUrl(d => d.AvatarUrl, s => s.AvatarUri);
    }
}

public class UserValueConverter(IServiceProvider services) : EntityCacheValueConverter<UserCache, UserBasicDto, Guid>(services);