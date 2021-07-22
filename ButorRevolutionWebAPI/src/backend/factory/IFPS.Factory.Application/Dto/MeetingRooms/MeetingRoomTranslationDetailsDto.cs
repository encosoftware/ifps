using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MeetingRoomTranslationDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LanguageTypeEnum Language { get; set; }

        public MeetingRoomTranslationDetailsDto(MeetingRoomTranslation modelObject)
        {
            this.Id = modelObject.Id;
            this.Name = modelObject.Name;
            this.Description = modelObject.Description;
            this.Language = modelObject.Language;
        }
    }
}