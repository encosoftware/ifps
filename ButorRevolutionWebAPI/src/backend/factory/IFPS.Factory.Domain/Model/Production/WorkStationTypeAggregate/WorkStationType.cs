using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class WorkStationType : AggregateRoot, IMultiLingualEntity<WorkStationTypeTranslation>
    {
        public WorkStationTypeEnum StationType { get; private set; }

        private List<WorkStationTypeTranslation> _translations;
        public ICollection<WorkStationTypeTranslation> Translations => _translations.AsReadOnly();

        public WorkStationTypeTranslation CurrentTranslation => (WorkStationTypeTranslation)Translations.GetCurrentTranslation();

        private WorkStationType()
        {
            _translations = new List<WorkStationTypeTranslation>();
        }

        public WorkStationType(WorkStationTypeEnum type) : this()
        {
            this.StationType = type;
        }

        public void AddTranslation(WorkStationTypeTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding WorkStationTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
