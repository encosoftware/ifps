using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ParticipantMessageConfiguration : EntityTypeConfiguration<ParticipantMessage>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ParticipantMessage> builder)
        {
            builder.HasOne(ent => ent.Message)
                .WithMany()
                .HasForeignKey(ent => ent.MessageId);

            builder.HasOne(ent => ent.Recipient)
                .WithMany(ent=>ent.ParticipantMessages)
                .HasForeignKey(ent => ent.RecipientId);
        }
    }
}
