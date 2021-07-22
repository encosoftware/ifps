using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class WorkStationTypeTranslationConfiguration : EntityTypeConfiguration<WorkStationTypeTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<WorkStationTypeTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
