using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class MachineConfiguration : EntityTypeConfiguration<Machine>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Machine> builder)
        {
            builder.HasOne(ent => ent.Brand)
                .WithMany()
                .HasForeignKey(ent => ent.BrandId);
        }
    }
}
