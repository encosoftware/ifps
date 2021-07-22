using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Report> builder)
        {
            builder.HasMany(ent => ent.StockReports)
                .WithOne(ent => ent.Report)
                .HasForeignKey(ent => ent.ReportId);

            builder.Metadata.FindNavigation(nameof(Report.StockReports)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
