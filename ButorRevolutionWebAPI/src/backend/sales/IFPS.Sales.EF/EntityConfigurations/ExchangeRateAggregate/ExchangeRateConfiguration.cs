using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ExchangeRateConfiguration : EntityTypeConfiguration<ExchangeRate>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.HasOne(ent => ent.BaseCurrency)
                .WithMany()
                .HasForeignKey(ent => ent.BaseCurrencyId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.ChangeCurrency)
                .WithMany()
                .HasForeignKey(ent => ent.ChangeCurrencyId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.ClientSetNull);
        }
    }
}
