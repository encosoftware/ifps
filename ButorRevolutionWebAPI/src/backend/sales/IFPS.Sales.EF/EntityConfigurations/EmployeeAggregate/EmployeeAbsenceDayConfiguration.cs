using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IFPS.Sales.EF.EntityConfigurations
{
    internal class EmployeeAbsenceDayConfiguration : EntityTypeConfiguration<EmployeeAbsenceDay>
    {
        public override void ConfigureEntity(EntityTypeBuilder<EmployeeAbsenceDay> builder)
        {
            builder.HasOne(ent => ent.Employee)
                .WithMany(u => u.AbsenceDays)
                .HasForeignKey(ent => ent.EmployeeId);                        
        }
    }
}

