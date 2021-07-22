using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class WorkStationConfiguration : EntityTypeConfiguration<WorkStation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WorkStation> builder)
        {
            builder.HasOne(ent => ent.Machine)
                .WithMany()
                .HasForeignKey(ent => ent.MachineId);

            builder.HasOne(ent => ent.WorkStationType)
                .WithMany()
                .HasForeignKey(ent => ent.WorkStationTypeId);

            builder.HasMany(ent => ent.WorkStationCameras)
                .WithOne(ent => ent.WorkStation)
                .HasForeignKey(ent => ent.WorkStationId);

            builder.HasMany(ent => ent.Plans)
                .WithOne(ent => ent.WorkStation)
                .HasForeignKey(ent => ent.WorkStationId);

            builder.Metadata.FindNavigation(nameof(WorkStation.WorkStationCameras)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(WorkStation.Plans)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
