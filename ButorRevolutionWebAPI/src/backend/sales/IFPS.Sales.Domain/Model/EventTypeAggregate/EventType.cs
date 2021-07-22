using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class EventType : AggregateRoot, IMultiLingualEntity<EventTypeTranslation>
    {
        /// <summary>
        /// Type of the event
        /// </summary>
        public EventTypeEnum Type { get; set; }

        /// <summary>
        /// Private list for managing translations
        /// </summary>
        private List<EventTypeTranslation> _translations;
        public ICollection<EventTypeTranslation> Translations => _translations.AsReadOnly();

        public EventTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private EventType()
        {
            _translations = new List<EventTypeTranslation>();
        }

        public EventType(EventTypeEnum type) : this()
        {
            Type = type;
        }

        public void AddTranslation(EventTypeTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding CountryTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
