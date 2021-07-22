using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.SiUnitAggregate
{
    public class SiUnitTranslationConfiguration : EntityTypeConfiguration<SiUnitTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<SiUnitTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
