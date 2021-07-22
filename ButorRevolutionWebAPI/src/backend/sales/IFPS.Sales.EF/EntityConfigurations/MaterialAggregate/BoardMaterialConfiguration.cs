using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class BoardMaterialConfiguration : IEntityTypeConfiguration<BoardMaterial>
    {
        public void Configure(EntityTypeBuilder<BoardMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));
            builder.OwnsOne(ent => ent.Dimension);
        }
    }
}