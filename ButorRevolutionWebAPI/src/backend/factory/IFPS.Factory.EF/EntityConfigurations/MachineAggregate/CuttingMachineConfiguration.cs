using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class CuttingMachineConfiguration : IEntityTypeConfiguration<CuttingMachine>
    {
        public void Configure(EntityTypeBuilder<CuttingMachine> builder)
        {
            builder.HasBaseType(typeof(Machine));
        }
    }
}
