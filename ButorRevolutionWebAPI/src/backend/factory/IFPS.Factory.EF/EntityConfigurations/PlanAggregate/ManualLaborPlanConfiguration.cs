using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ManualLaborPlanConfiguration : IEntityTypeConfiguration<ManualLaborPlan>
    {
        public void Configure(EntityTypeBuilder<ManualLaborPlan> builder)
        {
            builder.HasBaseType(typeof(Plan));

        }
    }
}
