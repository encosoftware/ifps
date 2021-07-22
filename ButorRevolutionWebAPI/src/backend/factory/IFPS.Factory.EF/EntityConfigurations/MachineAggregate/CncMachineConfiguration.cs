using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class CncMachineConfiguration : IEntityTypeConfiguration<CncMachine>
    {
        public void Configure(EntityTypeBuilder<CncMachine> builder)
        {
            builder.HasBaseType(typeof(Machine));

            builder.OwnsOne(ent => ent.EstimatorProperties);
            builder.OwnsOne(ent => ent.MillingProperties);
            builder.OwnsOne(ent => ent.DrillPropeties);

            builder.Metadata.FindNavigation(nameof(CncMachine.UnavailablePlanes)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(CncMachine.Tools)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
