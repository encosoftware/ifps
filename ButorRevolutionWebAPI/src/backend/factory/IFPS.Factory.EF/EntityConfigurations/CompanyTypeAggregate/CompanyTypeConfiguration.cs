using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CompanyTypeConfiguration : EntityTypeConfiguration<CompanyType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CompanyType> builder)
        {
            builder.Metadata.FindNavigation(nameof(CompanyType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
