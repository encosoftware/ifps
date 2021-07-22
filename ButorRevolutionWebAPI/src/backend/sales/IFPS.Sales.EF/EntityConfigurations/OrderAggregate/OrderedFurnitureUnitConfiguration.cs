using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class OrderedFurnitureUnitConfiguration : EntityTypeConfiguration<OrderedFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderedFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(ent => ent.OrderedFurnitureUnits)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.WebshopOrder)
                .WithMany(rev => rev.OrderedFurnitureUnits)
                .HasForeignKey(ent => ent.WebshopOrderId);

            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureUnitId);

            builder.HasOne(ent => ent.Basket)
                .WithMany(rev => rev.OrderedFurnitureUnits)
                .HasForeignKey(ent => ent.BasketId);

            builder.OwnsOne(ent => ent.UnitPrice);
        }
    }
}
