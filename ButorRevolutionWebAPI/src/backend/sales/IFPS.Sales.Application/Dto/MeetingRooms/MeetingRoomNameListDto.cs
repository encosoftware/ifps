using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class MeetingRoomNameListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MeetingRoomNameListDto(MeetingRoom meetingRoom)
        {
            Id = meetingRoom.Id;
            Name = meetingRoom.Name;
        }
    }

}
