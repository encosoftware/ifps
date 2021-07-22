using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class MeetingRoomConfiguration : EntityTypeConfiguration<MeetingRoom>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MeetingRoom> builder)
        {
            builder.HasOne(ent => ent.Venue)
                .WithMany(rev => rev.MeetingRooms)
                .HasForeignKey(ent => ent.VenueId);

            builder.Metadata.FindNavigation(nameof(MeetingRoom.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);            
        }
    }
}
