using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class FoilMaterialConfiguration : IEntityTypeConfiguration<FoilMaterial>
    {
        public void Configure(EntityTypeBuilder<FoilMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));
        }
    }
}