using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CncPlanConfiguration : IEntityTypeConfiguration<CncPlan>
    {
        public void Configure(EntityTypeBuilder<CncPlan> builder)
        {
            builder.HasBaseType(typeof(Plan));

        }
    }
}
