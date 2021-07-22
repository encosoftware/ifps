using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Language : AggregateRoot, IMultiLingualEntity<LanguageTranslation>
    {
        public LanguageTypeEnum LanguageType { get; set; }

        private List<LanguageTranslation> _translations;
        public ICollection<LanguageTranslation> Translations { get { return _translations; } private set { } }

        public LanguageTranslation CurrentTranslation => (LanguageTranslation)Translations.GetCurrentTranslation();

        public Language()
        {
            _translations = new List<LanguageTranslation>();
        }

        public Language(LanguageTypeEnum languageType) : this()
        {
            this.LanguageType = languageType;
        }

        public void AddTranslation(LanguageTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding LanguageTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
