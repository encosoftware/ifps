using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(ent => ent.Sender)
                .WithMany()
                .HasForeignKey(ent => ent.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.MessageChannel)
                .WithMany(ent => ent.Messages)
                .HasForeignKey(ent => ent.MessageChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.Metadata.FindNavigation(nameof(Message.Recipients)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
