using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DocumentConfiguration : EntityTypeGuidConfiguration<Document>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Document> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(ent => ent.UploadedBy)
                .WithMany()
                .HasForeignKey(ent => ent.UploadedById);

            builder.HasOne(ent => ent.DocumentType)
                .WithMany()
                .HasForeignKey(ent => ent.DocumentTypeId);

            builder.HasOne(ent => ent.DocumentGroupVersion)
                .WithMany(ent => ent.Documents)
                .HasForeignKey(ent => ent.DocumentGroupVersionId);

            builder.Property(ent => ent.FileName).IsRequired();
            builder.Property(ent => ent.ContainerName).IsRequired();
            builder.HasIndex(ent => ent.FileName).IsUnique();
        }
    }
}
