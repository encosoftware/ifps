using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class StockedMaterialConfiguration : EntityTypeConfiguration<StockedMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<StockedMaterial> builder)
        {
            builder.HasOne(ent => ent.Material)
                .WithMany()
                .HasForeignKey(ent => ent.MaterialId);
        }
    }
}