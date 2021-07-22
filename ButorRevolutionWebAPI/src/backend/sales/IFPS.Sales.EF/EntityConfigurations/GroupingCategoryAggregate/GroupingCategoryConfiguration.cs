
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class GroupingCategoryConfiguration : EntityTypeConfiguration<GroupingCategory>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GroupingCategory> builder)
        {
            builder.Metadata.FindNavigation(nameof(GroupingCategory.Translations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(ent => ent.ParentGroup)
                .WithMany(rev => rev.Children)
                .HasForeignKey(ent => ent.ParentGroupId);
            builder.Metadata.FindNavigation(nameof(GroupingCategory.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
