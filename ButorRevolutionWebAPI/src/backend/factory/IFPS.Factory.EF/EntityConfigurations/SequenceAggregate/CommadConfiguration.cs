using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class CommadConfiguration : EntityTypeConfiguration<Command>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Command> builder)
        {
            builder.OwnsOne(ent => ent.Point);

            builder.HasOne(ent => ent.Sequence)
                .WithMany()
                .HasForeignKey(ent => ent.SequenceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
