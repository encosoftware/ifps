using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.ExpenseAggregate
{
    public class FrequencyTypeTranslationsConfiguration : EntityTypeConfiguration<FrequencyTypeTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FrequencyTypeTranslation> builder)
        {
            builder.HasOne(entity => entity.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(entity => entity.CoreId);
        }
    }
}
