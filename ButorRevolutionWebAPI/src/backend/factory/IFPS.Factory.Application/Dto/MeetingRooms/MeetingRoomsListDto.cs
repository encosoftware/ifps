using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class MeetingRoomsListDto
    {
        public int Id { get; set; }

        public List<MeetingRoomTranslationDetailsDto> Translations { get; set; }

        public MeetingRoomsListDto(MeetingRoom meetingRoom)
        {
            this.Id = meetingRoom.Id;
            this.Translations = meetingRoom.Translations.Select(entity => new MeetingRoomTranslationDetailsDto(entity)).ToList();
        }
    }
}