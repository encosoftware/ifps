using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class MeetingRoom : FullAuditedAggregateRoot//FullAuditedEntity
    {
        /// <summary>
        /// Office building, where the room is located
        /// </summary>
        
        public virtual Venue Venue { get; set; }
        public int VenueId { get; set; }

        private List<MeetingRoomTranslation> _translations;
        public ICollection<MeetingRoomTranslation> Translations
        {
            get
            {
                return _translations;
            }
            set
            {
                if (value == null)
                {
                    throw new IFPSDomainException($"Error setting Translations, value is null.");
                }
                _translations = value.ToList();
            }
        }

        public MeetingRoomTranslation CurrentTranslation => (MeetingRoomTranslation)Translations.GetCurrentTranslation();

        private MeetingRoom()
        {
            _translations = new List<MeetingRoomTranslation>();            
        }

        public MeetingRoom(int officeBuildingId) : this()
        {
            this.VenueId = officeBuildingId;
        }

        public void AddTranslation(MeetingRoomTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding MeetingRoomTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
