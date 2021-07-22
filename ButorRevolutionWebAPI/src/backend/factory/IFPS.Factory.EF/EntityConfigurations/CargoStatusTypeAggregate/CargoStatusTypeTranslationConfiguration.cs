using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CargoStatusTypeTranslationConfiguration : IEntityTypeConfiguration<CargoStatusTypeTranslation>
    {
        public void Configure(EntityTypeBuilder<CargoStatusTypeTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}