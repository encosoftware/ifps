using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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
