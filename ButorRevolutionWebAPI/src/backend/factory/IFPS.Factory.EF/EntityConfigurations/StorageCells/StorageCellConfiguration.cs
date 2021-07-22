using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class StorageCellConfiguration : EntityTypeConfiguration<StorageCell>
    {
        public override void ConfigureEntity(EntityTypeBuilder<StorageCell> builder)
        {
            builder.HasOne(ent => ent.Warehouse)
                .WithMany(ent => ent.StorageCells)
                .HasForeignKey(ent => ent.WarehouseId);

            builder.HasMany(ent => ent.Stocks)
                .WithOne(ent => ent.StorageCell)
                .HasForeignKey(ent => ent.StorageCellId);

            builder.Metadata.FindNavigation(nameof(StorageCell.Stocks)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}