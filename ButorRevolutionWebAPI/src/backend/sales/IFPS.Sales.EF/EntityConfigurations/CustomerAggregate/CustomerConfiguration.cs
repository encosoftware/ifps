using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId);

            builder.HasMany(ent => ent.RecommendedProducts)
                .WithOne(ent=> ent.Customer)
                .HasForeignKey(ent => ent.CustomerId);

            builder.Metadata.FindNavigation(nameof(Customer.NotificationModes)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Customer.RecommendedProducts)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
