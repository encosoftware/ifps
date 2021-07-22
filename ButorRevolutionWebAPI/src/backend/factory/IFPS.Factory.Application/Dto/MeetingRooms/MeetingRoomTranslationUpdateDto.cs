using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MeetingRoomTranslationUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public LanguageTypeEnum Language { get; set; }

        public MeetingRoomTranslationUpdateDto()
        {
        }

        public MeetingRoomTranslationUpdateDto(MeetingRoomTranslation modelObject)
        {
            this.Name = modelObject.Name;
            this.Description = Description;
            this.Language = modelObject.Language;
        }
    }
}