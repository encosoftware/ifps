using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class EmailDataConfiguration : EntityTypeConfiguration<EmailData>
    {
        public override void ConfigureEntity(EntityTypeBuilder<EmailData> builder)
        {
            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(EmailData.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
