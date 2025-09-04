using Astra.EntityFrameworkCore.EntityPropertyConverters;
using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp;

namespace Crm.EntityFrameworkCore;

public static class CrmDbContextModelCreatingExtensions
{
    public static void ConfigureCrm(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<User>(options =>
        {
            options.Property(x => x.Name).HasColumnType("citext");
            options.Property(x => x.Email).HasColumnType("citext");
            
            options.HasIndex(x => x.Name).IsUnique();
            options.HasIndex(x => x.Email).IsUnique();
        });
        builder.Entity<Role>(options =>
        {
            options.Property(x => x.Id).HasMaxLength(32);
            options.Property(x => x.Name).HasMaxLength(32);
        });

        builder.Entity<Product>(options =>
        {
            options.Property(x => x.Id).HasMaxLength(64);
            options.Property(x => x.Name).HasMaxLength(64);
            options.Property(x => x.ImageUri).HasMaxLength(128);
            options.Property(x => x.Description).HasMaxLength(128);
        });
        builder.Entity<ProductSaleLog>(options =>
        {
            options.Property(x => x.CustomerEmail).HasColumnType("citext");
            options.Property(x => x.OrderNo).HasColumnType("citext");
            options.Property(x => x.Data)
                .HasColumnType("jsonb")
                .HasConversion(new JsonNodeConverter(), new JsonNodeComparer());
        });

        builder.Entity<CommissionLog>(options =>
        {
            options.Property(x => x.CustomerEmail).HasColumnType("citext");
            options.Property(x => x.ProductId).HasMaxLength(64);
            options.HasIndex(x => x.ReceiverId);
        });
        builder.Entity<ReferralLevel>(options =>
        {
            options.Property(x => x.Id).HasMaxLength(32);
            options.Property(x => x.Name).HasMaxLength(32);
            options.Property(x => x.Description).HasMaxLength(255);
        });
        builder.Entity<ReferralRelation>(options =>
        {
            options.OwnsOne(x => x.Recommender, opt =>
            {
                opt.Property(x => x.Id).HasColumnName("RecommenderId");
                opt.Property(x => x.Email)
                    .HasColumnName("RecommenderEmail")
                    .HasColumnType("citext");
            });
            options.OwnsOne(x => x.Recommendee, opt =>
            {
                opt.Property(x => x.Id).HasColumnName("RecommendeeId");
                opt.Property(x => x.Email)
                    .HasColumnName("RecommendeeEmail")
                    .HasColumnType("citext");
            });
        });
        builder.Entity<Referrer>(options =>
        {
            options.Property(x => x.LevelId).HasMaxLength(32);
            options.Property(x => x.WithdrawalAddress).HasColumnType("citext");
            options.Property(x => x.Remark).HasMaxLength(32);
            options.HasMany(x => x.Statistics)
                .WithOne(x => x.Referrer)
                .HasForeignKey(x => x.ReferrerId);
        });
        builder.Entity<ReferrerRequest>(options =>
        {
            options.Property(x => x.LevelId).HasMaxLength(32);
            options.Property(x => x.Status).HasConversion<EnumToStringConverter<ReferrerRequestStatus>>();
            options.Property(x => x.RejectReason).HasMaxLength(255);
        });
        builder.Entity<WithdrawalRequest>(options =>
        {
            options.Property(x => x.Status).HasConversion<EnumToStringConverter<WithdrawalRequestStatus>>();
            options.Property(x => x.ToAddress).HasColumnType("citext");
            options.Property(x => x.Txid).HasColumnType("citext");
            options.Property(x => x.RejectReason).HasMaxLength(255);
        });
    }
}
