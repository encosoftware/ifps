using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class DecorBoardMaterialConfiguration : IEntityTypeConfiguration<DecorBoardMaterial>
    {

        public void Configure(EntityTypeBuilder<DecorBoardMaterial> builder)
        {
            builder.HasBaseType(typeof(BoardMaterial));
        }
    }
}
