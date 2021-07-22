using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class FoilMaterialConfiguration : IEntityTypeConfiguration<FoilMaterial>
    {
        public void Configure(EntityTypeBuilder<FoilMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));
        }
    }
}