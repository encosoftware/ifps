using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class CircleConfiguration : IEntityTypeConfiguration<Circle>
    {
        public void Configure(EntityTypeBuilder<Circle> builder)
        {
            builder.HasBaseType(typeof(Sequence));

            builder.HasOne(ent => ent.StartPoint)
                .WithMany()
                .HasForeignKey(ent => ent.StartPointId);
        }
    }
}
