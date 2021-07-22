using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class OrderedMaterialPackageConfiguration : IEntityTypeConfiguration<OrderedMaterialPackage>
    {
        public void Configure(EntityTypeBuilder<OrderedMaterialPackage> builder)
        {
            builder.HasOne(ent => ent.MaterialPackage)
                .WithMany()
                .HasForeignKey(ent => ent.MaterialPackageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Cargo)
                .WithMany(rev => rev.OrderedPackages)
                .HasForeignKey(ent => ent.CargoId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.OwnsOne(ent => ent.UnitPrice);
        }
    }
}