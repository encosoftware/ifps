using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class CompanyType : AggregateRoot, IMultiLingualEntity<CompanyTypeTranslation>
    {       
        public CompanyTypeEnum Type { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<CompanyTypeTranslation> _translations;
        public ICollection<CompanyTypeTranslation> Translations => _translations.AsReadOnly();

        public CompanyTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private CompanyType()
        {
            _translations = new List<CompanyTypeTranslation>();
        }

        public CompanyType(CompanyTypeEnum type) : this()
        {
            Type = type;
        }

        public void AddTranslation(CompanyTypeTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding CompanyTypeTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
