using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class OrderStateTranslationConfiguration : EntityTypeConfiguration<OrderStateTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<OrderStateTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
