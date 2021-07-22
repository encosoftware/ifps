using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class OrderPriceConfiguration : EntityTypeConfiguration<OrderPrice>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderPrice> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany()
                .HasForeignKey(ent => ent.CoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.Price)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
