using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class DayTypeConfiguration : EntityTypeConfiguration<DayType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DayType> builder)
        {
            builder.Metadata.FindNavigation(nameof(DayType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
