using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class OrderedServiceConfiguration : EntityTypeConfiguration<OrderedService>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderedService> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(rev => rev.Services)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.Service)
                .WithMany()
                .HasForeignKey(ent => ent.ServiceId);
        }
    }
}
