using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class AccessoryMaterialFurnitureUnitConfiguration : EntityTypeConfiguration<AccessoryMaterialFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<AccessoryMaterialFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.Accessory)
                .WithMany()
                .HasForeignKey(ent => ent.AccessoryId);

            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany(rev => rev.Accessories)
                .HasForeignKey(ent => ent.FurnitureUnitId);
        }
    }
}
