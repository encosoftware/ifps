using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class ToolConfiguration : EntityTypeConfiguration<Tool>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Tool> builder)
        {
            builder.HasOne(ent => ent.CncMachine)
                .WithMany(ent => ent.Tools)
                .HasForeignKey(ent => ent.CncMachineId);
        }
    }
}
