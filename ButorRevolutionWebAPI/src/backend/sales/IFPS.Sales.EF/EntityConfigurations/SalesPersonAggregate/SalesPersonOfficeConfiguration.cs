using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class SalesPersonOfficeConfiguration : EntityTypeConfiguration<SalesPersonOffice>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SalesPersonOffice> builder)
        {
            builder.HasOne(ent => ent.SalesPerson)
                .WithMany(ent => ent.Offices)
                .HasForeignKey(ent => ent.SalesPersonId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Office)
                .WithMany()
                .HasForeignKey(ent => ent.OfficeId);

        }
    }
}
