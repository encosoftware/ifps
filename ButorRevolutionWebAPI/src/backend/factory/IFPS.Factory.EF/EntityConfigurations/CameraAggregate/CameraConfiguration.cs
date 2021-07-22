using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CameraConfiguration : EntityTypeConfiguration<Camera>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Camera> builder)
        {
            builder.HasOne(ent => ent.WorkStationCamera)
                .WithOne(ent=> ent.Camera)
                .HasForeignKey<WorkStationCamera>(ent => ent.CameraId);
        }
    }
}
