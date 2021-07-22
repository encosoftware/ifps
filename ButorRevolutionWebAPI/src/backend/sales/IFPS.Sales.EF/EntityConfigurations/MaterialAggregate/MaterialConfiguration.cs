using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class MaterialConfiguration : EntityTypeGuidConfiguration<Material>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Material> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Metadata.FindNavigation(nameof(Material.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(ent => ent.Image)
                .WithMany()
                .HasForeignKey(ent => ent.ImageId);

            builder.HasAlternateKey(c => c.Code);

            builder.HasOne(ent => ent.CurrentPrice)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Prices).WithOne(rev => rev.Core).HasForeignKey(rev => rev.CoreId);

            builder.Metadata.FindNavigation(nameof(Material.Prices)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}