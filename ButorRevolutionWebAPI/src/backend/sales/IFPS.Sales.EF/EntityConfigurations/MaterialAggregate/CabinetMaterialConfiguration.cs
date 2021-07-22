using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CabinetMaterialConfiguration : EntityTypeConfiguration<CabinetMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CabinetMaterial> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany()
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.InnerMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.InnerMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.OuterMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.OuterMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.BackPanelMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.BackPanelMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.DoorMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.DoorMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
