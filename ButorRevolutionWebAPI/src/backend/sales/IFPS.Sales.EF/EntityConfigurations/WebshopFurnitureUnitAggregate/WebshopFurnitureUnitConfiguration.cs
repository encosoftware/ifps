using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class WebshopFurnitureUnitConfiguration : EntityTypeConfiguration<WebshopFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WebshopFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureUnitId);

            builder.HasMany(ent => ent.Images)
                .WithOne(ent => ent.WebshopFurnitureUnit)
                .HasForeignKey(ent => ent.WebshopFurnitureUnitId);

            builder.OwnsOne(ent => ent.Price);

            builder.Metadata.FindNavigation(nameof(WebshopFurnitureUnit.Images)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
