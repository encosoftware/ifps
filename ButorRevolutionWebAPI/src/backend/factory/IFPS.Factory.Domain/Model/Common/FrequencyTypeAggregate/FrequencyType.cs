using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class FrequencyType : AggregateRoot, IMultiLingualEntity<FrequencyTypeTranslation>
    {
        public FrequencyTypeEnum Type { get; set; }

        private List<FrequencyTypeTranslation> _translations;
        public ICollection<FrequencyTypeTranslation> Translations
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

        public FrequencyTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private FrequencyType()
        {
            _translations = new List<FrequencyTypeTranslation>();
        }

        public FrequencyType(FrequencyTypeEnum type) : this()
        {
            Type = type;
        }

        public void AddTranslation(FrequencyTypeTranslation translation)
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
