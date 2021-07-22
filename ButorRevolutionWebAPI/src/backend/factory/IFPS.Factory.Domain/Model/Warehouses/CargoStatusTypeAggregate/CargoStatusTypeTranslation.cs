using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class CargoStatusTypeTranslation : Entity, IEntityTranslation<CargoStatusType>
    {
        public string Name { get; private set; }

        public CargoStatusType Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private CargoStatusTypeTranslation() { }

        public CargoStatusTypeTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
