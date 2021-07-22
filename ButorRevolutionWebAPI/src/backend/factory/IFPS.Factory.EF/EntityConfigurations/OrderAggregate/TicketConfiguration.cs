using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.OrderAggregate
{
    public class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(entity => entity.AssignedTo)
                .WithMany()
                .HasForeignKey(entity => entity.AssignedToId);

            builder.HasOne(ent => ent.Order)
                .WithMany(order => order.Tickets)
                .HasForeignKey(ent => ent.OrderId);
        }
    }
}
