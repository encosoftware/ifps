using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CuttingConfiguration : EntityTypeConfiguration<Cutting>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Cutting> builder)
        {
            builder.HasOne(ent => ent.LayoutPlan)
                .WithMany(rev => rev.Cuttings)
                .HasForeignKey(ent => ent.LayoutPlanId);
        }
    }
}
