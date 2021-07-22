using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DefaultRoleClaimsConfiguration : EntityTypeConfiguration<DefaultRoleClaim>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DefaultRoleClaim> builder)
        {
            builder.HasOne(ent => ent.Claim)
                .WithMany()
                .HasForeignKey(ent => ent.ClaimId);

            builder.HasOne(ent => ent.Role)
                .WithMany(role => role.DefaultRoleClaims)
                .HasForeignKey(ent => ent.RoleId);
        }
    }
}
