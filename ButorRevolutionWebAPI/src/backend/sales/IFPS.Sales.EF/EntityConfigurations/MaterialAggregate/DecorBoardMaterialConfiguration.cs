using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DecorBoardMaterialConfiguration : IEntityTypeConfiguration<DecorBoardMaterial>
    {

        public void Configure(EntityTypeBuilder<DecorBoardMaterial> builder)
        {
            builder.HasBaseType(typeof(BoardMaterial));
        }
    }
}
