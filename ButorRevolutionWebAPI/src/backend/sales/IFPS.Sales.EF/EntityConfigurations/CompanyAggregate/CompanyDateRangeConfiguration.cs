using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CompanyDateRangeConfiguration : EntityTypeConfiguration<CompanyDateRange>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CompanyDateRange> builder)
        {
            builder.HasOne(ent => ent.Company)
                .WithMany(ent => ent.OpeningHours)
                .HasForeignKey(ent => ent.CompanyId);

            builder.OwnsOne(ent => ent.Interval);
        }
    }
}
