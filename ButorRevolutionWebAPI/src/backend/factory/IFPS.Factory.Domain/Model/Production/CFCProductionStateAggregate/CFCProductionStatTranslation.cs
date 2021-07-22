using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class CFCProductionStateTranslation : Entity, IEntityTranslation<CFCProductionState>
    {
        public string Name { get; private set; }

        public CFCProductionState Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private CFCProductionStateTranslation() { }

        public CFCProductionStateTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
