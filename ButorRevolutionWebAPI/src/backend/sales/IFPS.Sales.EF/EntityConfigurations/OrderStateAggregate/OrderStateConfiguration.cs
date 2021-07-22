using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class OrderStateConfiguration : EntityTypeConfiguration<OrderState>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderState> builder)
        {
            builder.Metadata.FindNavigation(nameof(OrderState.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
