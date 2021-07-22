using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CorpusMaterialConfiguration : EntityTypeConfiguration<CorpusMaterial>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CorpusMaterial> builder)
        {
            builder.OwnsOne(ent => ent.Dimensions);

            builder.HasOne(ent => ent.WallMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.WallMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.InnerMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.InnerMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.BackPanelMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.BackPanelMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.FrontMaterial)
                .WithMany()
                .HasForeignKey(ent => ent.FrontMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}