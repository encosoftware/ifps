using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ProductionProcessConfiguration : EntityTypeConfiguration<ProductionProcess>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ProductionProcess> builder)
        {
            builder.HasMany(ent => ent.Workers)
                .WithOne(ent => ent.Process)
                .HasForeignKey(ent => ent.ProcessId);

            builder.HasOne(ent => ent.Plan)
                .WithOne(ent => ent.ProductionProcess)
                .HasForeignKey<ProductionProcess>(ent => ent.PlanId);


            builder.Metadata.FindNavigation(nameof(ProductionProcess.Workers)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
