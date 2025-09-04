using AutoMapper;
using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;

namespace Crm;

public class EntityCacheMapperProfile : Profile
{
    public EntityCacheMapperProfile()
    {
        CreateMap<User, UserCache>();
        CreateMap<Role, RoleCache>();
        CreateMap<Product, ProductCache>();
        CreateMap<ReferralLevel, ReferralLevelCache>();
    }
}