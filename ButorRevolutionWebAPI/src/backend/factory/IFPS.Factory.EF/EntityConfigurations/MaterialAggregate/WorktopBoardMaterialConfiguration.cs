using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class WorktopBoardMaterialConfiguration : IEntityTypeConfiguration<WorktopBoardMaterial>
    {
        public void Configure(EntityTypeBuilder<WorktopBoardMaterial> builder)
        {
            builder.HasBaseType(typeof(BoardMaterial));
        }
    }
}
