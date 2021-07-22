using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class WorktopBoardMaterialConfiguration : IEntityTypeConfiguration<WorktopBoardMaterial>
    {

        public void Configure(EntityTypeBuilder<WorktopBoardMaterial> builder)
        {
            builder.HasBaseType(typeof(BoardMaterial));
        }
    }
}
