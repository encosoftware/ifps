using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Role> builder)
        {
            builder.Metadata.FindNavigation(nameof(Role.DefaultRoleClaims)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne(ent => ent.Division)
                .WithMany()
                .HasForeignKey(ent => ent.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
