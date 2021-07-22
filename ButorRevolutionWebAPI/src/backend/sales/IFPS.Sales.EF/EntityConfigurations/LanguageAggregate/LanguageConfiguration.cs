using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class LanguageConfiguration : EntityTypeConfiguration<Language>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Language> builder)
        {
            builder.Metadata.FindNavigation(nameof(Language.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
