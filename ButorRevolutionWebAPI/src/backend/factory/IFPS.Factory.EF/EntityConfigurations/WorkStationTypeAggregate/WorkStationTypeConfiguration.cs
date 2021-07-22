using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class WorkStationTypeConfiguration : EntityTypeConfiguration<WorkStationType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WorkStationType> builder)
        {
            builder.Metadata.FindNavigation(nameof(WorkStationType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
              .HasForeignKey(ent => ent.CoreId);
        }
    }
}
