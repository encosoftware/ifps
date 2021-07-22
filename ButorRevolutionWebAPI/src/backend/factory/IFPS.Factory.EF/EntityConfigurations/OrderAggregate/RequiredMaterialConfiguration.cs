using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.OrderAggregate
{
    public class RequiredMaterialConfiguration : EntityTypeConfiguration<RequiredMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<RequiredMaterial> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany()
                .HasForeignKey(ent => ent.OrderId);

            builder.HasOne(ent => ent.Material)
                .WithMany()
                .HasForeignKey(ent => ent.MaterialId);
        }
    }
}
