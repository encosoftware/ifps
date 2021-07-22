using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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
