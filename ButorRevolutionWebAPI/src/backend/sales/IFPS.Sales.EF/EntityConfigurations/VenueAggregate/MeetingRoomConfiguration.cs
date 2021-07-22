using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class MeetingRoomConfiguration : EntityTypeConfiguration<MeetingRoom>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MeetingRoom> builder)
        {
            builder.HasOne(ent => ent.Venue)
                .WithMany(rev => rev.MeetingRooms)
                .HasForeignKey(ent => ent.VenueId);

            builder.HasMany(ent => ent.Appointments)
                .WithOne(rev => rev.MeetingRoom)
                .HasForeignKey(ent => ent.MeetingRoomId);

            builder.Metadata.FindNavigation(nameof(MeetingRoom.Appointments)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
