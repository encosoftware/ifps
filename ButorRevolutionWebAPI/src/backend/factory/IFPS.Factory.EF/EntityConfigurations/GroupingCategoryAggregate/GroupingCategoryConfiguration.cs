using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class GroupingCategoryConfiguration : EntityTypeConfiguration<GroupingCategory>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GroupingCategory> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.ParentGroup)
                .WithMany(rev => rev.Children)
                .HasForeignKey(ent => ent.ParentGroupId);

            builder.Metadata.FindNavigation(nameof(GroupingCategory.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
