using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class EventTypeConfiguration : EntityTypeConfiguration<EventType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<EventType> builder)
        {
            builder.Metadata.FindNavigation(nameof(EventType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
