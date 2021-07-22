
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class SalesPersonConfiguration : EntityTypeConfiguration<SalesPerson>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SalesPerson> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Supervisor)
                .WithMany(rev => rev.Supervisees)
                .HasForeignKey(ent => ent.SupervisorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Metadata.FindNavigation(nameof(SalesPerson.Supervisees)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(SalesPerson.Offices)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(SalesPerson.DefaultTimeTable)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(ent => ent.MaxDiscount).HasDefaultValue(0);
            builder.Property(ent => ent.MinDiscount).HasDefaultValue(0);
        }
    }
}
