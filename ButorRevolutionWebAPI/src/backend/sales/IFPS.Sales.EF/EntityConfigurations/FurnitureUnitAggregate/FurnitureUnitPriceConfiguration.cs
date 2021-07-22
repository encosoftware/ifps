using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class FurnitureUnitPriceConfiguration : EntityTypeConfiguration<FurnitureUnitPrice>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FurnitureUnitPrice> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Prices)
                .HasForeignKey(ent => ent.CoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.Price)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.MaterialCost)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
