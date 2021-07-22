using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class MessageChannelParticipantConfiguration : EntityTypeConfiguration<MessageChannelParticipant>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MessageChannelParticipant> builder)
        {
            builder.HasOne(ent => ent.MessageChannel)
                .WithMany(ent => ent.Participants)
                .HasForeignKey(ent => ent.MessageChannelId);

            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.ParticipantMessages)
                .WithOne(ent => ent.Recipient)
                .HasForeignKey(ent => ent.RecipientId);

            builder.Metadata.FindNavigation(nameof(MessageChannelParticipant.ParticipantMessages)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
