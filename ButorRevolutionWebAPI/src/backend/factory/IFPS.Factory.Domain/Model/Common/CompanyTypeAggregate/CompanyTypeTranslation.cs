using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class CompanyTypeTranslation : Entity, IEntityTranslation<CompanyType>
    {
        public string Name { get; private set; }

        public CompanyType Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private CompanyTypeTranslation() { }

        public CompanyTypeTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
