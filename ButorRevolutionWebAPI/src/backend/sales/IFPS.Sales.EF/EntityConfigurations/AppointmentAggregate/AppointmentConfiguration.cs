using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class AppointmentConfiguration : EntityTypeConfiguration<Appointment>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(ent => ent.Customer)
                .WithMany()
                .HasForeignKey(ent => ent.CustomerId);

            builder.HasOne(ent => ent.AnonymousUser)
                .WithMany()
                .HasForeignKey(ent => ent.AnonymousUserId);

            builder.HasOne(ent => ent.Partner)
                .WithMany()
                .HasForeignKey(ent => ent.PartnerId);

            builder.HasOne(ent => ent.Order)
                .WithMany()
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.Category)
                .WithMany()
                .HasForeignKey(ent => ent.CategoryId);

            builder.HasOne(ent => ent.CanceledBy)
                .WithMany()
                .HasForeignKey(ent => ent.CanceledById);

            builder.OwnsOne(ent => ent.Address);

            builder.OwnsOne(ent => ent.ScheduledDateTime);
        }
    }
}
