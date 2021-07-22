using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DocumentGroupConfiguration : EntityTypeConfiguration<DocumentGroup>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentGroup> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(ent => ent.DocumentGroups)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.DocumentFolder)
                .WithMany()
                .HasForeignKey(ent => ent.DocumentFolderId);

            builder.Metadata.FindNavigation(nameof(DocumentGroup.Versions)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
