using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class BasketConfiguration : EntityTypeConfiguration<Basket>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Basket> builder)
        {
            builder.HasOne(ent => ent.Customer)
                .WithMany()
                .HasForeignKey(ent => ent.CustomerId);

            builder.HasOne(ent => ent.Email)
                .WithMany()
                .HasForeignKey(ent => ent.EmailId);

            builder.HasOne(ent => ent.Service)
                .WithMany()
                .HasForeignKey(ent => ent.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.OrderedFurnitureUnits)
                .WithOne(ent => ent.Basket)
                .HasForeignKey(rev => rev.BasketId);

            builder.OwnsOne(ent => ent.SubTotal)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.DelieveryPrice)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.DelieveryAddress);
            builder.OwnsOne(ent => ent.BillingAddress);


            builder.Metadata.FindNavigation(nameof(Basket.OrderedFurnitureUnits)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
