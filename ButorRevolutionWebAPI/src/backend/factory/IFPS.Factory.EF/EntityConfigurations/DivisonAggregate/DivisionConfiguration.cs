using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class DivisionConfiguration : EntityTypeConfiguration<Division>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Division> builder)
        {
            builder.HasMany(ent => ent.Claims)
                .WithOne(ent => ent.Division)
                .HasForeignKey(ent => ent.DivisionId);

            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(Division.Claims))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata.FindNavigation(nameof(Division.Translations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
