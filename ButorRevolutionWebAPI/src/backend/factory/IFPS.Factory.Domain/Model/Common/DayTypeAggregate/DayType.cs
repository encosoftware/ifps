using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class DayType : AggregateRoot, IMultiLingualEntity<DayTypeTranslation>
    {
        public DayTypeEnum Type { get; private set; }
        public int Order { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<DayTypeTranslation> _translations;
        public ICollection<DayTypeTranslation> Translations => _translations.AsReadOnly();

        public DayTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private DayType()
        {
            _translations = new List<DayTypeTranslation>();
        }

        public DayType(DayTypeEnum type, int order) : this()
        {
            Type = type;
            Order = order;
        }

        public void AddTranslation(DayTypeTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding DayTypeTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
