using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class VenueConfiguration : EntityTypeConfiguration<Venue>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Venue> builder)
        {
            builder.HasOne(ent => ent.Company)
                .WithMany()
                .HasForeignKey(ent => ent.CompanyId);

            builder.OwnsOne(ent => ent.OfficeAddress);

            builder.Metadata.FindNavigation(nameof(Venue.MeetingRooms)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Venue.OpeningHours)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
