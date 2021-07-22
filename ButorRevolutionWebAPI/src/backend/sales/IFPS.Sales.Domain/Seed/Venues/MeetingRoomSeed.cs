using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Venues
{
    public class MeetingRoomSeed : IEntitySeed<MeetingRoom>
    {
        public MeetingRoom[] Entities => new[]
        {
            new MeetingRoom("Kék tárgyaló (seed)", "B épület második emelet jobbra", 1) { Id = 1 },
            new MeetingRoom("Nagy tárgyaló (seed)", "B épület második emelet balra", 1) { Id = 2 }
        };
    }
}
