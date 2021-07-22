using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class EdgingMachineConfiguration : IEntityTypeConfiguration<EdgingMachine>
    {
        public void Configure(EntityTypeBuilder<EdgingMachine> builder)
        {
            builder.HasBaseType(typeof(Machine));
        }
    }
}
