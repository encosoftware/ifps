using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.OrderAggregate
{
    public class MaterialPackageConfiguration : IEntityTypeConfiguration<MaterialPackage>
    {
        public void Configure(EntityTypeBuilder<MaterialPackage> builder)
        {
            builder.HasBaseType(typeof(Package));

            builder.HasOne(ent => ent.Material)
                .WithMany(rev => rev.Packages)
                .HasForeignKey(ent => ent.MaterialId);

            builder.HasOne(ent => ent.Supplier)
                .WithMany()
                .HasForeignKey(ent => ent.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.Price)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
