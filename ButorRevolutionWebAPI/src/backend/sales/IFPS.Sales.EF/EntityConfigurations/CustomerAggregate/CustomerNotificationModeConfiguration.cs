using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CustomerNotificationModeConfiguration : EntityTypeConfiguration<CustomerNotificationMode>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CustomerNotificationMode> builder)
        {
            builder.HasOne(ent => ent.Customer)
                .WithMany(rev => rev.NotificationModes)
                .HasForeignKey(ent => ent.CustomerId);

            builder.HasOne(ent => ent.EventType)
                .WithMany()
                .HasForeignKey(ent => ent.EventTypeId);
        }
    }
}
