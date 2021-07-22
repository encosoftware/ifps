using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ConcreteApplianceMaterialConfiguration : EntityTypeConfiguration<ConcreteApplianceMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ConcreteApplianceMaterial> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(rev => rev.ConcreteApplianceMaterials)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.ApplianceMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.ApplianceMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
