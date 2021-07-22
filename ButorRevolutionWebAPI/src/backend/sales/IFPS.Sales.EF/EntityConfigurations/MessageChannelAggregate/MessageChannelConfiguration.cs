using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class MessageChannelConfiguration : EntityTypeConfiguration<MessageChannel>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MessageChannel> builder)
        {
            builder.HasOne(ent => ent.Order)
                .WithMany()
                .HasForeignKey(ent => ent.OrderId);

            builder.HasMany(ent => ent.Participants)
                .WithOne(ent=>ent.MessageChannel)
                .HasForeignKey(ent => ent.MessageChannelId);

            builder.Metadata.FindNavigation(nameof(MessageChannel.Participants)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(MessageChannel.Messages)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
