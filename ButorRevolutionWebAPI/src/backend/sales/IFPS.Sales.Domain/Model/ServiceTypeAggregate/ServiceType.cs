using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFPS.Sales.Domain.Model
{
    public class ServiceType : AggregateRoot, IMultiLingualEntity<ServiceTypeTranslation>
    {
        public ServiceTypeEnum Type { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<ServiceTypeTranslation> _translations;
        public ICollection<ServiceTypeTranslation> Translations
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

        public ServiceTypeTranslation CurrentTranslation => (ServiceTypeTranslation)Translations.GetCurrentTranslation();

        private ServiceType()
        {
            _translations = new List<ServiceTypeTranslation>();
        }

        public ServiceType(ServiceTypeEnum type) : this()
        {
            this.Type = type;
        }

        public void AddTranslation(ServiceTypeTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding ServiceTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
