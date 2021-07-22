using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CustomerFurnitureUnitConfiguration : EntityTypeConfiguration<CustomerFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CustomerFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.Customer)
                .WithMany(ent => ent.RecommendedProducts)
                .HasForeignKey(ent => ent.CustomerId);

            builder.HasOne(ent => ent.WebshopFurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.WebshopFurnitureUnitId);
        }
    }
}
