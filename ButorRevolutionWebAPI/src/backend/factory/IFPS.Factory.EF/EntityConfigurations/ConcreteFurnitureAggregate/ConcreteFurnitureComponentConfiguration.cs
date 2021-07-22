using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ConcreteFurnitureComponentConfiguration : EntityTypeConfiguration<ConcreteFurnitureComponent>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ConcreteFurnitureComponent> builder)
        {
            builder.HasOne(ent => ent.ConcreteFurnitureUnit)
                .WithMany(rev => rev.ConcreteFurnitureComponents)
                .HasForeignKey(ent => ent.ConcreteFurnitureUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.FurnitureComponent)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Cutting)
                .WithMany()
                .HasForeignKey(ent => ent.CuttingId);

            builder.HasOne(ent => ent.QRCode)
                .WithMany()
                .HasForeignKey(ent => ent.QRCodeId);
        }
    }
}

