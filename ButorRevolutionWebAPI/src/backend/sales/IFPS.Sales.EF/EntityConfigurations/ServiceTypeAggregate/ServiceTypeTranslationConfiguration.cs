using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ServiceTypeTranslationConfiguration : EntityTypeConfiguration<ServiceTypeTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ServiceTypeTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany()
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
