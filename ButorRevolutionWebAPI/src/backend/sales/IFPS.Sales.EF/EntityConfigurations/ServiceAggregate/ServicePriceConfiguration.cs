using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ServicePriceConfiguration : EntityTypeConfiguration<ServicePrice>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ServicePrice> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Prices)
                .HasForeignKey(ent => ent.CoreId);

            builder.OwnsOne(ent => ent.Price)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
