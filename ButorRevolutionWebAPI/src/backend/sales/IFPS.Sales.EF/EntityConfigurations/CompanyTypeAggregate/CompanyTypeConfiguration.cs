using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CompanyTypeConfiguration : EntityTypeConfiguration<CompanyType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CompanyType> builder)
        {
            builder.Metadata.FindNavigation(nameof(CompanyType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
