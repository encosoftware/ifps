using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class OrderedApplianceMaterialConfiguration : EntityTypeConfiguration<OrderedApplianceMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderedApplianceMaterial> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(rev => rev.OrderedApplianceMaterials)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.ApplianceMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.ApplianceMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
