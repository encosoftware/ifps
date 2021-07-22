using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class MeetingRoomsListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MeetingRoomsListDto(MeetingRoom meetingRoom)
        {
            Id = meetingRoom.Id;
            Name = meetingRoom.Name;
            Description = meetingRoom.Description;
        }
    }
}
