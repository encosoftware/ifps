using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class DocumentStateConfiguration : EntityTypeConfiguration<DocumentState>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentState> builder)
        {
            builder.Metadata.FindNavigation(nameof(DocumentState.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
