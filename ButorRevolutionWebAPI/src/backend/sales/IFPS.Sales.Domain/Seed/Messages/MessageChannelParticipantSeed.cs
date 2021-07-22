using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class MessageChannelParticipantSeed : IEntitySeed<MessageChannelParticipant>
    {
        public MessageChannelParticipant[] Entities => new[]
        {
            new MessageChannelParticipant(1,1) {Id = 1},
            new MessageChannelParticipant(1,3) {Id = 2},
            new MessageChannelParticipant(2,1) {Id = 3},
            new MessageChannelParticipant(2,3) {Id = 4}
        };
        //public MessageChannelParticipant[] Entities => new MessageChannelParticipant[] { };
    }
}
