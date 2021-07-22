using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class SortingStrategyTypeConfiguration : EntityTypeConfiguration<SortingStrategyType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SortingStrategyType> builder)
        {
            builder.Metadata.FindNavigation(nameof(SortingStrategyType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
