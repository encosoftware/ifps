using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    internal class SalesPersonDateRangeConfiguration : EntityTypeConfiguration<SalesPersonDateRange>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SalesPersonDateRange> builder)
        {
            builder.HasOne(ent => ent.SalesPerson)
                .WithMany(ent => ent.DefaultTimeTable)
                .HasForeignKey(ent => ent.SalesPersonId);

            builder.OwnsOne(ent => ent.Interval);                
        }
    }
}
