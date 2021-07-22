using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class RelativePointConfiguration : EntityTypeConfiguration<RelativePoint>
    {
        public override void ConfigureEntity(EntityTypeBuilder<RelativePoint> builder)
        {
            builder.OwnsOne(ent => ent.X);
            builder.OwnsOne(ent => ent.Y);
            builder.OwnsOne(ent => ent.Z);
        }
    }
}
