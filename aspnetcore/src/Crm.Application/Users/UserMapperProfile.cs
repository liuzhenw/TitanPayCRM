using Astra.AutoMapper;
using AutoMapper;
using Crm.Accounts;

namespace Crm.Users;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.HasPassword, o => o.MapFrom(s => s.PasswordHash != null))
            .ToUrl(d => d.AvatarUrl, s => s.AvatarUri);
    }
}