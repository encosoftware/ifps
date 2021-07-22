using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class AccessoryMaterialFurnitureUnitConfiguration : EntityTypeConfiguration<AccessoryMaterialFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<AccessoryMaterialFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany(rev => rev.Accessories)
                .HasForeignKey(ent => ent.FurnitureUnitId);

            builder.HasOne(ent => ent.Accessory)
                .WithMany()
                .HasForeignKey(ent => ent.AccessoryId);
        }
    }
}
