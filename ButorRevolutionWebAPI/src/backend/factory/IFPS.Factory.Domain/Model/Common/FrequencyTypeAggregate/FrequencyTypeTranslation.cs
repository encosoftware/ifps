using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class FrequencyTypeTranslation : Entity, IEntityTranslation<FrequencyType>
    {
        public string Name { get; set; }

        public LanguageTypeEnum Language { get; set; }

        public FrequencyType Core { get; private set; }
        public int CoreId { get; private set; }

        public FrequencyTypeTranslation(string name, int coreId, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
