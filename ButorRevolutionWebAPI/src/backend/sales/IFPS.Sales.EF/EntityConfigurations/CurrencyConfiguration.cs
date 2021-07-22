using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CurrencyConfiguration : EntityTypeConfiguration<Currency>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Currency> builder)
        {

        }
    }
}
