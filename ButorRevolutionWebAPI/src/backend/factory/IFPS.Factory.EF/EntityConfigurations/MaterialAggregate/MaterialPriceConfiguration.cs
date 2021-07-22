using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class MaterialPriceConfiguration : EntityTypeConfiguration<MaterialPrice>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MaterialPrice> builder)
        {
            builder.OwnsOne(ent => ent.Price);

            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Prices)
                .HasForeignKey(ent => ent.CoreId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
