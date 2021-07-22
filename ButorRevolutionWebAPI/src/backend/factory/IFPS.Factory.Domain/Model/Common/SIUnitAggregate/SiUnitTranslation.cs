using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class SiUnitTranslation : Entity, IEntityTranslation<SiUnit>
    {
        public string UnitName { get; set; }

        public SiUnit Core { get; private set; }
        public int CoreId { get; private set; }
        public LanguageTypeEnum Language { get; private set; }

        private SiUnitTranslation()
        {

        }

        public SiUnitTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            this.UnitName = name;
            this.CoreId = coreId;
            this.Language = language;
        }
    }
}
