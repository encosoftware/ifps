using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId);

            builder.Metadata.FindNavigation(nameof(Employee.AbsenceDays)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
