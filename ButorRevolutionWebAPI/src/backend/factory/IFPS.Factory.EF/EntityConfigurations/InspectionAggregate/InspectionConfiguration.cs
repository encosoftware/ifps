using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class InspectionConfiguration : EntityTypeConfiguration<Inspection>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Inspection> builder)
        {
            builder.HasOne(ent => ent.InspectedStorage)
                .WithMany()
                .HasForeignKey(ent => ent.InspectedStorageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.Report)
                .WithMany()
                .HasForeignKey(ent => ent.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull);          


            builder.Metadata.FindNavigation(nameof(Inspection.UserInspections)).SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
