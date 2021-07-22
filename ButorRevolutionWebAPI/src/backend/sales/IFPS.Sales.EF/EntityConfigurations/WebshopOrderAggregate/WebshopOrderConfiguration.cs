using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    internal class WebshopOrderConfiguration : EntityTypeGuidConfiguration<WebshopOrder>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WebshopOrder> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.Customer)
                .WithMany()
                .HasForeignKey(ent => ent.CustomerId);

            builder.HasMany(ent => ent.OrderedFurnitureUnits)
                .WithOne(ent=> ent.WebshopOrder)
                .HasForeignKey(ent => ent.WebshopOrderId);

            builder.HasMany(ent => ent.Services)
                .WithOne(ent=> ent.WebshopOrder)
                .HasForeignKey(ent => ent.WebshopOrderId);

            builder.HasAlternateKey(ent => new { ent.WorkingNumberSerial, ent.WorkingNumberYear }).HasName("X_WorkingNumber");

            builder.OwnsOne(ent => ent.ShippingAddress);

            builder.Metadata.FindNavigation(nameof(Order.OrderedFurnitureUnits)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.Services)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
