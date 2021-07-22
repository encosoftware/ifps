using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class MeetingRoomSeed : IEntitySeed<MeetingRoom>
    {
        public MeetingRoom[] Entities => new[]
        {
            new MeetingRoom(1) { Id = 1 },
            new MeetingRoom(1) { Id = 2 }
        };
    }
}
