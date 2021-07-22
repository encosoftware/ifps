using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ServiceTypeConfiguration : EntityTypeConfiguration<ServiceType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ServiceType> builder)
        {
            builder.Metadata.FindNavigation(nameof(ServiceType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
