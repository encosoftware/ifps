
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserNotificationTextConfiguration : EntityTypeConfiguration<UserNotificationText>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserNotificationText> builder)
        {
            builder.HasOne(ent => ent.Customer)
                .WithMany()
                .HasForeignKey(ent => ent.CustomerId);
        }
    }
}
