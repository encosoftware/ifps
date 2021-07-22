using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class DocumentFolderConfiguration : EntityTypeConfiguration<DocumentFolder>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentFolder> builder)
        {
            builder.Metadata.FindNavigation(nameof(DocumentFolder.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(DocumentFolder.DocumentTypes)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
