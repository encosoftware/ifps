using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CurrencyConfiguration : EntityTypeConfiguration<Currency>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Currency> builder)
        {

        }
    }
}
