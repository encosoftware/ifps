using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Country> builder)
        {
            builder.Metadata.FindNavigation(nameof(Country.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
