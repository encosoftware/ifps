using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class SortingStrategyTypeTranslation : Entity, IEntityTranslation<SortingStrategyType>
    {
        public string Name { get; private set; }

        public SortingStrategyType Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private SortingStrategyTypeTranslation()
        {

        }

        public SortingStrategyTypeTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
