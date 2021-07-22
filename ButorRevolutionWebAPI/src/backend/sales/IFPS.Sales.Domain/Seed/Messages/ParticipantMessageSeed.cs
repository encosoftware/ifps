using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ParticipantMessageSeed : IEntitySeed<ParticipantMessage>
    {
        public ParticipantMessage[] Entities => new[]
        {
            new ParticipantMessage(1,3) {Id = 1},
            new ParticipantMessage(2,3) {Id = 2},
            new ParticipantMessage(3,3) {Id = 3},
            new ParticipantMessage(4,3) {Id = 4},
            new ParticipantMessage(5,3) {Id = 5},
            new ParticipantMessage(6,3) {Id = 6}
        };
        //public ParticipantMessage[] Entities => new ParticipantMessage[] { };
    }
}
