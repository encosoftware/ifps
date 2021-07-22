using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class CFCProductionState : AggregateRoot, IMultiLingualEntity<CFCProductionStateTranslation>
    {
        public CFCProductionStateEnum State { get; private set; }

        private List<CFCProductionStateTranslation> _translations;
        public ICollection<CFCProductionStateTranslation> Translations => _translations.AsReadOnly();

        public CFCProductionStateTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private CFCProductionState()
        {
            _translations = new List<CFCProductionStateTranslation>();
        }

        public CFCProductionState(CFCProductionStateEnum state) : this()
        {
            State = state;
        }

        public void AddTranslation(CFCProductionStateTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding CFCProductionStateTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
