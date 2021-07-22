using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations.OrderAggregate
{
    public class OfferInformationConfiguration : EntityTypeConfiguration<OfferInformation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OfferInformation> builder)
        {
            var priceBuilder = builder.OwnsOne(ent => ent.ProductsPrice);
            priceBuilder = builder.OwnsOne(ent => ent.ServicesPrice);
            priceBuilder.HasOne<Currency>()
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            priceBuilder = builder.OwnsOne(ent => ent.ServicesPrice);
            priceBuilder.HasOne<Currency>()
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
