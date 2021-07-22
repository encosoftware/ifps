using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DocumentGroupVersionConfiguration : EntityTypeConfiguration<DocumentGroupVersion>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentGroupVersion> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Versions)
                .HasForeignKey(ent => ent.CoreId);

            builder.HasOne(ent => ent.State)
                .WithMany()
                .HasForeignKey(ent => ent.StateId);

            builder.Metadata.FindNavigation(nameof(DocumentGroupVersion.Documents)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
