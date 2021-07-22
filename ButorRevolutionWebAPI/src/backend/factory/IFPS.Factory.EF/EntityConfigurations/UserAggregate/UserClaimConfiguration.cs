using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany(user => user.Claims)
                .HasForeignKey(ent => ent.UserId);

            builder.HasOne(ent => ent.Claim)
                .WithMany()
                .HasForeignKey(ent => ent.ClaimId);
        }
    }
}
