using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class AccessoryMaterialConfiguration : IEntityTypeConfiguration<AccessoryMaterial>
    {
        public void Configure(EntityTypeBuilder<AccessoryMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));
        }
    }
}
