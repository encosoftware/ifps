using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DocumentTypeConfiguration : EntityTypeConfiguration<DocumentType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentType> builder)
        {
            builder.Metadata.FindNavigation(nameof(DocumentType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne(ent => ent.DocumentFolder)
                .WithMany(ent=> ent.DocumentTypes)
                .HasForeignKey(ent => ent.DocumentFolderId);
        }
    }
}
