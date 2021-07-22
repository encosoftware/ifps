using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.EF.EntityConfigurations.PackageAggregate
{
    public class OrderedMaterialPackageConfiguration : IEntityTypeConfiguration<OrderedMaterialPackage>
    {
        public void Configure(EntityTypeBuilder<OrderedMaterialPackage> builder)
        {
            builder.HasOne(ent => ent.MaterialPackage)
                .WithMany()
                .HasForeignKey(ent => ent.MaterialPackageId);

            builder.HasOne(ent => ent.Cargo)
                .WithMany()
                .HasForeignKey(ent => ent.CargoId);

            builder.OwnsOne(ent => ent.UnitPrice)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
