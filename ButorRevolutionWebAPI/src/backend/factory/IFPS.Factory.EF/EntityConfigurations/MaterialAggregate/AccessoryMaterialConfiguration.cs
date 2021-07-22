using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class AccessoryMaterialConfiguration : IEntityTypeConfiguration<AccessoryMaterial>
    {
        public void Configure(EntityTypeBuilder<AccessoryMaterial> builder)
        {
            builder.HasBaseType(typeof(Material));
        }
    }
}
