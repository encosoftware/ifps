using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class CompanyType : AggregateRoot, IMultiLingualEntity<CompanyTypeTranslation>
    {
        public CompanyTypeEnum Type { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<CompanyTypeTranslation> _translations;
        public ICollection<CompanyTypeTranslation> Translations
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
