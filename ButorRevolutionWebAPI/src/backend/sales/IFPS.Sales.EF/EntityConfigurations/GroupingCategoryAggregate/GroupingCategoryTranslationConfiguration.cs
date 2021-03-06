using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class GroupingCategoryTranslationConfiguration : EntityTypeConfiguration<GroupingCategoryTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GroupingCategoryTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(ent => ent.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
