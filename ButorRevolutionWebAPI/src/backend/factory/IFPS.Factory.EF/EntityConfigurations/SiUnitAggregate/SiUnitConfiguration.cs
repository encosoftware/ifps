using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.SiUnitAggregate
{
    public class SiUnitConfiguration : EntityTypeConfiguration<SiUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SiUnit> builder)
        {
            builder.HasMany(ent => ent.Translations)
           .WithOne(ent => ent.Core)
           .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(SiUnit.Translations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
