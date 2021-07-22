using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ServiceConfiguration : EntityTypeConfiguration<Service>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Service> builder)
        {
            builder.HasOne(ent => ent.CurrentPrice)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.ServiceType)
                .WithMany()
                .HasForeignKey(ent => ent.ServiceTypeId);

            builder.HasMany(ent => ent.Prices)
                .WithOne(rev => rev.Core)
                .HasForeignKey(rev => rev.CoreId);

            builder.Metadata.FindNavigation(nameof(Service.Prices)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
