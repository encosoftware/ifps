using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class VenueConfiguration : EntityTypeConfiguration<Venue>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Venue> builder)
        {

            builder.OwnsOne(ent => ent.OfficeAddress);

            builder.Metadata.FindNavigation(nameof(Venue.MeetingRooms))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Venue.OpeningHours))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
