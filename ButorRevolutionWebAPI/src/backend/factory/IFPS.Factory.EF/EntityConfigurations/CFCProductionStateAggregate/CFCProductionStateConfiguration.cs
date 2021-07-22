using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CFCProductionStateConfiguration : EntityTypeConfiguration<CFCProductionState>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CFCProductionState> builder)
        {
            builder.Metadata.FindNavigation(nameof(CFCProductionState.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
              .HasForeignKey(ent => ent.CoreId);
        }
    }
}
