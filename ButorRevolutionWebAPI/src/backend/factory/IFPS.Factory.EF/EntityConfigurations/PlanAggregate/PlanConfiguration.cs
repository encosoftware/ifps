using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class PlanConfiguration : EntityTypeConfiguration<Plan>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Plan> builder)
        {
            builder.HasOne(ent => ent.WorkStation)
                .WithMany(ent => ent.Plans)
                .HasForeignKey(ent => ent.WorkStationId);

            builder.HasOne(ent => ent.ProductionProcess)
                .WithOne(ent => ent.Plan)
                .HasForeignKey<Plan>(ent => ent.ProductionProcessId);

            builder.HasOne(ent => ent.ConcreteFurnitureComponent)
                .WithMany()
                .HasForeignKey(ent => ent.ConcreteFurnitureComponentId);

            builder.HasOne(ent => ent.ConcreteFurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.ConcreteFurnitureUnitId);

            builder.HasOne(ent => ent.Optimization)
                .WithMany(ent => ent.Plans)
                .HasForeignKey(ent => ent.OptimizationId);
        }
    }
}
