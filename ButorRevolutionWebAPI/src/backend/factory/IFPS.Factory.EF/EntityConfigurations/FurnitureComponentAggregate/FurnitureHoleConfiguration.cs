using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class FurnitureHoleConfiguration : EntityTypeConfiguration<FurnitureHole>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FurnitureHole> builder)
        {
            builder.HasOne(ent => ent.FurnitureComponent)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureComponentId);
            
            builder.HasOne(ent => ent.StartPoint)
                .WithMany()
                .HasForeignKey(ent => ent.StartPointId);

            builder.HasOne(ent => ent.EndPoint)
                .WithMany()
                .HasForeignKey(ent => ent.EndPointId);
        }
    }
}
