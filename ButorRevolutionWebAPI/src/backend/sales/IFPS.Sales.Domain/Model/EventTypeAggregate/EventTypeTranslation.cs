using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class EventTypeTranslation : Entity, IEntityTranslation<EventType>
    {
        /// <summary>
        /// Name of the event type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the event type
        /// </summary>
        public string Description { get; set; }

        public virtual EventType Core { get; set; }
        public int CoreId { get; set; }

        public LanguageTypeEnum Language { get; set; }

        public EventTypeTranslation(int coreId, string name, string description, LanguageTypeEnum language)
        {
            CoreId = coreId;
            Name = name;
            Description = description;
            Language = language;
        }
    }
}
