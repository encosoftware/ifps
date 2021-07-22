using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class PolygonConfiguration : IEntityTypeConfiguration<Polygon>
    {
        public void Configure(EntityTypeBuilder<Polygon> builder)
        {
            builder.HasBaseType(typeof(Sequence));

            builder.HasOne(ent => ent.StartPoint)
                .WithMany()
                .HasForeignKey(ent => ent.StartPointId);

            builder.Metadata.FindNavigation(nameof(Polygon.Commands)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
