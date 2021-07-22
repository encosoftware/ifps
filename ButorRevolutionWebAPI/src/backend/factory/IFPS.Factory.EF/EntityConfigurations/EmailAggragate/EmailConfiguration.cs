using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class EmailConfiguration : EntityTypeConfiguration<Email>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Email> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany(ent => ent.Emails)
                .HasForeignKey(ent => ent.UserId);

            builder.HasOne(ent => ent.EmailData)
                .WithMany()
                .HasForeignKey(ent => ent.EmailDataId);
        }
    }
}
