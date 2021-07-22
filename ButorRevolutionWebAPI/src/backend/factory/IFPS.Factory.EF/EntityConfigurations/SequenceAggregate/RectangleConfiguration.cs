using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class RectangleConfiguration : IEntityTypeConfiguration<Rectangle>
    {
        public void Configure(EntityTypeBuilder<Rectangle> builder)
        {
            builder.HasBaseType(typeof(Sequence));

            builder.OwnsOne(ent => ent.BottomLeft);

            builder.OwnsOne(ent => ent.BottomRight);

            builder.OwnsOne(ent => ent.TopLeft);

            builder.OwnsOne(ent => ent.TopRight);
        }
    }
}
