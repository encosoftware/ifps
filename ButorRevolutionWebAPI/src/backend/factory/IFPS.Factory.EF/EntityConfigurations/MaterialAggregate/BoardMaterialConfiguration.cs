using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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