
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class FurnitureComponentConfiguration : EntityTypeGuidConfiguration<FurnitureComponent>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FurnitureComponent> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany(rev => rev.Components)
                .HasForeignKey(ent => ent.FurnitureUnitId);

            builder.HasOne(ent => ent.BoardMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.BoardMaterialId);

            builder.HasOne(ent => ent.TopFoil)
                .WithMany()
                .HasForeignKey(ent => ent.TopFoilId);

            builder.HasOne(ent => ent.RightFoil)
                .WithMany()
                .HasForeignKey(ent => ent.RightFoilId);

            builder.HasOne(ent => ent.BottomFoil)
                .WithMany()
                .HasForeignKey(ent => ent.BottomFoilId);

            builder.HasOne(ent => ent.LeftFoil)
                .WithMany()
                .HasForeignKey(ent => ent.LeftFoilId);

            builder.HasOne(ent => ent.Image)
                .WithMany()
                .HasForeignKey(ent => ent.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
