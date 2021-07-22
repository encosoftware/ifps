using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Role> builder)
        {
            builder.HasOne(ent => ent.Division)
                .WithMany()
                .HasForeignKey(ent => ent.DivisionId);

            builder.HasMany(ent => ent.DefaultRoleClaims)
                .WithOne(ent => ent.Role)
                .HasForeignKey(ent => ent.RoleId);

            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(Role.DefaultRoleClaims)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Role.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
