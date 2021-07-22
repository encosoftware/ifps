using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class HoleConfiguration : IEntityTypeConfiguration<Hole>
    {
        public void Configure(EntityTypeBuilder<Hole> builder)
        {
            builder.HasBaseType(typeof(Command));

            builder.HasOne(ent => ent.Drill)
                .WithMany(rev => rev.Holes)
                .HasForeignKey(ent => ent.DrillId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
