using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.ExpenseAggregate
{
    public class FrequencyTypeConfiguration : EntityTypeConfiguration<FrequencyType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FrequencyType> builder)
        {
            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(FrequencyType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
