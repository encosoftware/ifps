using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DayTypeConfiguration : EntityTypeConfiguration<DayType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DayType> builder)
        {
            builder.Metadata.FindNavigation(nameof(DayType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
