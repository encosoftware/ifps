using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class WebshopFurnitureUnitImageConfiguration : EntityTypeConfiguration<WebshopFurnitureUnitImage>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WebshopFurnitureUnitImage> builder)
        {
            builder.HasOne(ent => ent.WebshopFurnitureUnit)
                .WithMany(rev => rev.Images)
                .HasForeignKey(ent => ent.WebshopFurnitureUnitId);

            builder.HasOne(ent => ent.Image)
                .WithMany()
                .HasForeignKey(ent => ent.ImageId);
        }
    }
}
