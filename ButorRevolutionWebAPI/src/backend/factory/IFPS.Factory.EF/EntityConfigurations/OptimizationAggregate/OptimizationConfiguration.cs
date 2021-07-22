using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class OptimizatonConfiguration : EntityTypeGuidConfiguration<Optimization>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Optimization> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Metadata.FindNavigation(nameof(Optimization.Plans)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Optimization.Orders)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(ent => ent.LayoutPlanZip)
                .WithOne(file => file.Optimization)
                .HasForeignKey<LayoutPlanZipFile>(file => file.OptimizationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.ScheduleZip)
                .WithOne(file => file.Optimization)
                .HasForeignKey<ScheduleZipFile>(file => file.OptimizationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
