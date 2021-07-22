using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Country : AggregateRoot, IMultiLingualEntity<CountryTranslation>
    {
        /// <summary>
        /// Country Code (for example, HU for Hungary and UK for United Kingdom)    
        /// </summary>
        public string Code { get; private set; }

        private List<CountryTranslation> _translations;
        public ICollection<CountryTranslation> Translations
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

        public CountryTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private Country()
        {
            _translations = new List<CountryTranslation>();
        }

        public Country(string code) : this()
        {
            Code = code;
        }

        public void AddTranslation(CountryTranslation translation)
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
