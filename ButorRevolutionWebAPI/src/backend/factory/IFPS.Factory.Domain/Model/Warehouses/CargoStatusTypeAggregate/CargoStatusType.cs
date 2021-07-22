using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class CargoStatusType : AggregateRoot, IMultiLingualEntity<CargoStatusTypeTranslation>
    {
        public CargoStatusEnum Status { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<CargoStatusTypeTranslation> _translations;
        public ICollection<CargoStatusTypeTranslation> Translations => _translations.AsReadOnly();

        public CargoStatusTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private CargoStatusType()
        {
            _translations = new List<CargoStatusTypeTranslation>();
        }

        public CargoStatusType(CargoStatusEnum status) : this()
        {
            Status = status;
        }

        public void AddTranslation(CargoStatusTypeTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding CargoStatusTypeTranslation: duplicate language: {translation.Language}");
            }
            _translations.Add(translation);
        }

    }
}
