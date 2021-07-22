using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ConcreteFurnitureUnitConfiguration : EntityTypeConfiguration<ConcreteFurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ConcreteFurnitureUnit> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany(ent => ent.ConcreteFurnitureUnits)
                .HasForeignKey(ent => ent.OrderId);

            builder.HasMany(ent => ent.ConcreteFurnitureComponents)
                .WithOne(ent => ent.ConcreteFurnitureUnit)
                .HasForeignKey(ent => ent.ConcreteFurnitureUnitId);

            builder.HasOne(ent => ent.FurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.FurnitureUnitId);

            builder.HasMany(ent => ent.Plans)
                .WithOne(ent => ent.ConcreteFurnitureUnit)
                .HasForeignKey(ent => ent.ConcreteFurnitureUnitId);

            builder.Metadata.FindNavigation(nameof(ConcreteFurnitureUnit.Plans)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(ConcreteFurnitureUnit.ConcreteFurnitureComponents)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}

