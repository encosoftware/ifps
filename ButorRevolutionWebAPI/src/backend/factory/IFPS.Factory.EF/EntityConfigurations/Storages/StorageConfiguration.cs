using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class StorageConfiguration : EntityTypeConfiguration<Storage>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Storage> builder)
        {
            builder.HasOne(ent => ent.Company)
                .WithMany()
                .HasForeignKey(ent => ent.CompanyId);

            builder.OwnsOne(ent => ent.Address);

            builder.HasMany(ent => ent.StorageCells)
                .WithOne(ent => ent.Warehouse)
                .HasForeignKey(ent => ent.WarehouseId);

            builder.Metadata.FindNavigation(nameof(Storage.StorageCells)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}