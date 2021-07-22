using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.OrderAggregate
{
    public class OrderConfiguration : EntityTypeGuidConfiguration<Order>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Order> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.FirstPayment)
                .WithMany()
                .HasForeignKey(ent => ent.FirstPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.SecondPayment)
                 .WithMany()
                 .HasForeignKey(ent => ent.SecondPaymentId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Tickets)
                .WithOne(rev => rev.Order)
                .HasForeignKey(rev => rev.OrderId);

            builder.HasOne(ent => ent.Company)
                .WithMany()
                .HasForeignKey(ent => ent.CompanyId);

            builder.HasOne(ent => ent.CurrentTicket)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentTicketId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Optimization)
                .WithMany(opt => opt.Orders)
                .HasForeignKey(ent => ent.OptimizationId);

            builder.Metadata.FindNavigation(nameof(Order.Tickets)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.FurnitureUnits)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.ConcreteFurnitureUnits)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.ConcreteApplianceMaterials)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Order.Documents)).SetPropertyAccessMode(PropertyAccessMode.Field);
            //builder.Metadata.FindNavigation(nameof(Order.Stocks)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(ent => ent.ShippingAddress);
        }
    }
}
