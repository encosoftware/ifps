using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ApplianceMaterialConfiguration : IEntityTypeConfiguration<ApplianceMaterial>
    {
        public void Configure(EntityTypeBuilder<ApplianceMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));

            builder.HasOne(ent => ent.Brand)
                .WithMany()
                .HasForeignKey(ent => ent.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Distributor)
                .WithMany()
                .HasForeignKey(ent => ent.DistributorId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.OwnsOne(ent => ent.SellPrice);
        }
    }
}