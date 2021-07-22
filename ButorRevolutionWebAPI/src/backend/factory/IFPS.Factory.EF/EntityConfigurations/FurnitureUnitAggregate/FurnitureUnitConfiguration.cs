using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class FurnitureUnitConfiguration : EntityTypeGuidConfiguration<FurnitureUnit>
    {
        public override void ConfigureEntity(EntityTypeBuilder<FurnitureUnit> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.Image)
                .WithMany()
                .HasForeignKey(ent => ent.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Metadata.FindNavigation(nameof(FurnitureUnit.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(FurnitureUnit.Components)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(FurnitureUnit.Accessories)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(ent => ent.Category)
                .WithMany()
                .HasForeignKey(ent => ent.CategoryId);

            builder.HasOne(ent => ent.BaseFurnitureUnit)
                .WithMany()
                .HasForeignKey(ent => ent.BaseFurnitureUnitId);

            builder.HasOne(ent => ent.CurrentPrice)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Prices).WithOne(rev => rev.Core).HasForeignKey(rev => rev.CoreId);

            builder.Metadata.FindNavigation(nameof(FurnitureUnit.Prices)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}