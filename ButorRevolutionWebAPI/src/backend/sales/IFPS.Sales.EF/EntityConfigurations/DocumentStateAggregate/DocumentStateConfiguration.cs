using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DocumentStateConfiguration : EntityTypeConfiguration<DocumentState>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentState> builder)
        {
            builder.Metadata.FindNavigation(nameof(DocumentState.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
