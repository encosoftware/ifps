using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class LayoutPlanConfiguration : IEntityTypeConfiguration<LayoutPlan>
    {
        public void Configure(EntityTypeBuilder<LayoutPlan> builder)
        {
            builder.HasBaseType(typeof(Plan));

            builder.HasOne(ent => ent.Board)
                .WithMany()
                .HasForeignKey(ent => ent.BoardId); 

            builder.Metadata.FindNavigation(nameof(LayoutPlan.Cuttings)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
