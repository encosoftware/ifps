using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class GrooveConfiguration : EntityTypeConfiguration<Groove>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Groove> builder)
        {
            builder.HasOne(ent => ent.FurnitureComponent)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureComponentId);
        }
    }
}
