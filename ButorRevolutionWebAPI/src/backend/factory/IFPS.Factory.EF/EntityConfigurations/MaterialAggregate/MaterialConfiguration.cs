using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class MaterialConfiguration : EntityTypeGuidConfiguration<Material>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Material> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.Image)
                .WithMany()
                .HasForeignKey(ent => ent.ImageId);

            builder.HasOne(ent => ent.Category)
                .WithMany()
                .HasForeignKey(ent => ent.CategoryId);

            builder.HasOne(ent => ent.CurrentPrice)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Prices)
                .WithOne(rev => rev.Core)
                .HasForeignKey(rev => rev.CoreId);

            builder.HasOne(ent => ent.SiUnit)
                .WithMany()
                .HasForeignKey(ent => ent.SiUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Metadata.FindNavigation(nameof(Material.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Material.Prices)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Material.Packages)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}