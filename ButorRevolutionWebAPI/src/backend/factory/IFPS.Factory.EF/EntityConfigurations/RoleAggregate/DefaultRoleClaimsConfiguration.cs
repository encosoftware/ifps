using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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
                .HasForeignKey(ent => ent.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
