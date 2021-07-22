using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CargoConfiguration : EntityTypeConfiguration<Cargo>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Cargo> builder)
        {
            builder.HasOne(ent => ent.Supplier)
                .WithMany()
                .HasForeignKey(ent => ent.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Status)
                .WithMany()
                .HasForeignKey(ent => ent.StatusId);

            builder.HasOne(ent => ent.BookedBy)
                .WithMany()
                .HasForeignKey(ent => ent.BookedById);

            builder.OwnsOne(ent => ent.NetCost)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.ShippingCost)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.Vat)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.ShippingAddress);

            builder.Metadata.FindNavigation(nameof(Cargo.OrderedPackages)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}