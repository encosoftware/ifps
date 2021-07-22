using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class LayoutPlanZipFileConfiguration : IEntityTypeConfiguration<LayoutPlanZipFile>
    {
        public void Configure(EntityTypeBuilder<LayoutPlanZipFile> builder)
        {
            
        }
    }
}
