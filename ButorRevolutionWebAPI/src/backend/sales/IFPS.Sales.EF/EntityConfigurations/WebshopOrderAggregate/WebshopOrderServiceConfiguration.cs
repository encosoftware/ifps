using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class WebshopOrderServiceConfiguration : EntityTypeConfiguration<WebshopOrderService>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WebshopOrderService> builder)
        {
            builder.HasOne(ent => ent.WebshopOrder)
                .WithMany(rev => rev.Services)
                .HasForeignKey(ent => ent.WebshopOrderId);

            builder.HasOne(ent => ent.Service)
                .WithMany()
                .HasForeignKey(ent => ent.ServiceId);
        }
    }
}
