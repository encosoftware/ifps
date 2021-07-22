using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class VenueDateRangeConfiguration : EntityTypeConfiguration<VenueDateRange>
    {
        public override void ConfigureEntity(EntityTypeBuilder<VenueDateRange> builder)
        {
            builder.HasOne(ent => ent.Venue)
                .WithMany(rev => rev.OpeningHours)
                .HasForeignKey(ent => ent.VenueId);

            builder.HasOne(ent => ent.DayType)
                .WithMany()
                .HasForeignKey(ent => ent.DayTypeId);

            builder.OwnsOne(ent => ent.Interval);
        }
    }
}
