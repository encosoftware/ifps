using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class MeetingRoomTranslation : Entity, IEntityTranslation<MeetingRoom>
    {
        /// <summary>
        /// Name or number of the room
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path, how to get to the meeting room
        /// </summary>
        public string Description { get; set; }

        public LanguageTypeEnum Language { get; set; }

        public MeetingRoom Core { get; set; }
        public int CoreId { get; set; }

        private MeetingRoomTranslation()
        {

        }
        public MeetingRoomTranslation(int coreId, string name, string description, LanguageTypeEnum language)
        {
            this.Name = name;
            this.Description = description;
            this.CoreId = coreId;
            this.Language = language;
        }

    }
}
