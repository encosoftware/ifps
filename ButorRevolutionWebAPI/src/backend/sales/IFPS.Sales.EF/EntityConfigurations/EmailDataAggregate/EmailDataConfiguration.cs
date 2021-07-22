using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class EmailDataConfiguration : EntityTypeConfiguration<EmailData>
    {
        public override void ConfigureEntity(EntityTypeBuilder<EmailData> builder)
        {
            builder.Metadata.FindNavigation(nameof(EmailData.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
