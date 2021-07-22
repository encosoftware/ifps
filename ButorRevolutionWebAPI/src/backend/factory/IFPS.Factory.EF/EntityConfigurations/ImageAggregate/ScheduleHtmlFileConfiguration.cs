using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ScheduleHtmlFileConfiguration : IEntityTypeConfiguration<ScheduleZipFile>
    {
        public void Configure(EntityTypeBuilder<ScheduleZipFile> builder)
        {
            
        }
    }
}
