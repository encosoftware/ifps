using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class StockConfiguration : EntityTypeConfiguration<Stock>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Stock> builder)
        {
            builder.HasOne(ent => ent.Package)
                .WithMany()
                .HasForeignKey(ent => ent.PackageId);

            builder.HasOne(ent => ent.StorageCell)
                .WithMany(ent => ent.Stocks)
                .HasForeignKey(ent => ent.StorageCellId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}