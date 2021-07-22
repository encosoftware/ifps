using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class Country : AggregateRoot, IMultiLingualEntity<CountryTranslation>
    {
        /// <summary>
        /// Country Code (for example, HU for Hungary and UK for United Kingdom)    
        /// </summary>
        public string Code { get; private set; }

        private List<CountryTranslation> _translations;
        public ICollection<CountryTranslation> Translations => _translations.AsReadOnly();

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
