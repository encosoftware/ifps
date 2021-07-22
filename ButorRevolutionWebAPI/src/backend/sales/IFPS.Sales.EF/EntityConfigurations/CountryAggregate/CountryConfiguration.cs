using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Country> builder)
        {
            builder.Metadata.FindNavigation(nameof(Country.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
