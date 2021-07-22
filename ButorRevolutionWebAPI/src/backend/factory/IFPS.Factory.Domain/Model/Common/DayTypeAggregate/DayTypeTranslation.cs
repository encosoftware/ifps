using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class DayTypeTranslation : Entity, IEntityTranslation<DayType>
    {
        public string Name { get; private set; }

        public DayType Core { get; private set; }
        public int CoreId { get; private set; }
        
        public LanguageTypeEnum Language { get; private set; }

        private DayTypeTranslation() { }

        public DayTypeTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
