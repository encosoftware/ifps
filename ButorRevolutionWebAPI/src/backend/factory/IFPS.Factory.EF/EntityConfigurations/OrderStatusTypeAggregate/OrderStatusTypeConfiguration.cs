using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class OrderStatusTypeConfiguration : EntityTypeConfiguration<OrderState>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderState> builder)
        {
            builder.Metadata.FindNavigation(nameof(OrderState.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
