using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ProcessWorkerConfiguration : EntityTypeConfiguration<ProcessWorker>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ProcessWorker> builder)
        {
            builder.HasOne(ent => ent.Process)
                .WithMany(process => process.Workers)
                .HasForeignKey(ent => ent.ProcessId);

            builder.HasOne(ent => ent.Worker)
                .WithMany()
                .HasForeignKey(ent => ent.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
