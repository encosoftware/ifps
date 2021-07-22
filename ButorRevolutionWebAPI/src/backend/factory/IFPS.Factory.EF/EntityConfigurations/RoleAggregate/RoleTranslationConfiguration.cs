using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class RoleTranslationConfiguration : EntityTypeConfiguration<RoleTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<RoleTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(ent => ent.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
