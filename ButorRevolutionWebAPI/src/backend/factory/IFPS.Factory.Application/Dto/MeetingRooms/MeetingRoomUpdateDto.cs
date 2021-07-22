using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class MeetingRoomUpdateDto
    {
        public int? Id { get; set; }
        public IList<MeetingRoomTranslationUpdateDto> Translations { get; set; }
    }
}