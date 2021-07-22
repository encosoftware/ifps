using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using ENCO.DDD.Extensions;

namespace IFPS.Factory.Domain.Model
{
    public class SiUnit : AggregateRoot, IMultiLingualEntity<SiUnitTranslation>
    {
        public SiUnitEnum UnitType { get; set; }

        private List<SiUnitTranslation> _translations;
        public ICollection<SiUnitTranslation> Translations
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

        public SiUnitTranslation CurrentTranslation => (SiUnitTranslation)Translations.GetCurrentTranslation();

        private SiUnit()
        {
            _translations = new List<SiUnitTranslation>();
        }

        public SiUnit(SiUnitEnum unitType ) : this()
        {
            this.UnitType = unitType;
        }

        public void AddTranslation(SiUnitTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding CountryTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
