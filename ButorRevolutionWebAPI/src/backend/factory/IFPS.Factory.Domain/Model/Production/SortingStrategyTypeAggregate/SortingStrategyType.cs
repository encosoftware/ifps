using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class SortingStrategyType : AggregateRoot, IMultiLingualEntity<SortingStrategyTypeTranslation>
    {
        public SortingStrategyTypeEnum StrategyType { get; private set; }

        private List<SortingStrategyTypeTranslation> _translations;
        public ICollection<SortingStrategyTypeTranslation> Translations => _translations.AsReadOnly();

        public SortingStrategyTypeTranslation CurrentTranslation => (SortingStrategyTypeTranslation)Translations.GetCurrentTranslation();

        private SortingStrategyType()
        {
            _translations = new List<SortingStrategyTypeTranslation>();
        }

        public SortingStrategyType(SortingStrategyTypeEnum type) : this()
        {
            this.StrategyType = type;
        }

        public void AddTranslation(SortingStrategyTypeTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding SortingStrategyTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
