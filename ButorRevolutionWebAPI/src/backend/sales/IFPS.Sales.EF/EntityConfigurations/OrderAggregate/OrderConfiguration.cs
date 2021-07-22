using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    internal class OrderConfiguration : EntityTypeGuidConfiguration<Order>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Order> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.Customer)
                .WithMany()
                .HasForeignKey(ent => ent.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.SalesPerson)
                .WithMany()
                .HasForeignKey(ent => ent.SalesPersonId);

            builder.HasOne(ent => ent.FirstPayment)
                .WithMany()
                .HasForeignKey(ent => ent.FirstPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.SecondPayment)
                 .WithMany()
                 .HasForeignKey(ent => ent.SecondPaymentId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.CurrentTicket)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentTicketId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.TopCabinet)
                .WithMany()
                .HasForeignKey(ent => ent.TopCabinetId);

            builder.HasOne(ent => ent.BaseCabinet)
                .WithMany()
                .HasForeignKey(ent => ent.BaseCabinetId);

            builder.HasOne(ent => ent.TallCabinet)
                .WithMany()
                .HasForeignKey(ent => ent.TallCabinetid);

            builder.HasMany(ent => ent.OrderedFurnitureUnits)
                .WithOne(ent => ent.Order)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasMany(ent => ent.Services)
                .WithOne(ent=> ent.Order)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasMany(ent => ent.Tickets)
                .WithOne(ent => ent.Order)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.OfferInformation)
                .WithMany()
                .HasForeignKey(ent => ent.OfferInformationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasAlternateKey(ent => new { ent.WorkingNumberSerial, ent.WorkingNumberYear }).HasName("IX_WorkingNumber");

            builder.Metadata.FindNavigation(nameof(Order.Tickets)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.OrderedFurnitureUnits)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.OrderedApplianceMaterials)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.Services)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.DocumentGroups)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(ent => ent.ShippingAddress);
            builder.OwnsOne(ent => ent.Budget);
            //builder.

        }
    }
}
