using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class Language : AggregateRoot, IMultiLingualEntity<LanguageTranslation>
    {
        public LanguageTypeEnum LanguageType { get; set; }

        private List<LanguageTranslation> _translations;
        public ICollection<LanguageTranslation> Translations => _translations.AsReadOnly();

        public LanguageTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        public Language()
        {
            _translations = new List<LanguageTranslation>();
        }

        public Language(LanguageTypeEnum languageType) : this()
        {
            LanguageType = languageType;
        }

        public void AddTranslation(LanguageTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding LanguageTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}