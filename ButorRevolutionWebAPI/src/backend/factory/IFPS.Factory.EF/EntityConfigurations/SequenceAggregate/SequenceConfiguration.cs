using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class SequenceConfiguration : EntityTypeConfiguration<Sequence>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Sequence> builder)
        {
            builder.HasOne(ent => ent.FurnitureComponent)
                .WithMany(rev => rev.Sequences)
                .HasForeignKey(ent => ent.FurnitureComponentId);
        }
    }
}
