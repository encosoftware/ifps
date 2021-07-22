using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class DrillConfiguration : IEntityTypeConfiguration<Drill>
    {
        public void Configure(EntityTypeBuilder<Drill> builder)
        {
            builder.HasBaseType(typeof(Sequence));

            builder.HasMany(ent => ent.Holes)
                .WithOne(rev => rev.Drill)
                .HasForeignKey(ent => ent.DrillId);

            builder.Metadata.FindNavigation(nameof(Drill.Holes)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
