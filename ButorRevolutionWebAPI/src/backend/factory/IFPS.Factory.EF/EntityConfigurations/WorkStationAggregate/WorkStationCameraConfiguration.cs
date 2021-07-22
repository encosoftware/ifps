using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class WorkStationCameraConfiguration : EntityTypeConfiguration<WorkStationCamera>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WorkStationCamera> builder)
        {
            builder.HasOne(ent => ent.WorkStation)
                .WithMany(ent => ent.WorkStationCameras)
                .HasForeignKey(ent => ent.WorkStationId);

            builder.HasOne(ent => ent.Camera)
                .WithMany()
                .HasForeignKey(ent => ent.CameraId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.CFCProductionState)
                .WithMany()
                .HasForeignKey(ent => ent.CFCProductionStateId);
        }
    }
}
