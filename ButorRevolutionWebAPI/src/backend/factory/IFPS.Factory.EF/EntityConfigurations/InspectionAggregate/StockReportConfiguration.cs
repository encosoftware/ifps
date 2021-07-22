using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class StockReportConfiguration : EntityTypeConfiguration<StockReport>
    {
        public override void ConfigureEntity(EntityTypeBuilder<StockReport> builder)
        {
            builder.HasOne(ent => ent.Stock)
                .WithMany()
                .HasForeignKey(ent => ent.StockId);

            builder.HasOne(ent => ent.Report)
                .WithMany(ent => ent.StockReports)
                .HasForeignKey(ent => ent.ReportId);
        }
    }
}
