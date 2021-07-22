using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    internal class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(order => order.Tickets)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.AssignedTo)
                .WithMany()
                .HasForeignKey(ent => ent.AssignedToId);

            builder.HasOne(ent => ent.OrderState)
                .WithMany()
                .HasForeignKey(ent => ent.OrderStateId);
        }
    }
}
