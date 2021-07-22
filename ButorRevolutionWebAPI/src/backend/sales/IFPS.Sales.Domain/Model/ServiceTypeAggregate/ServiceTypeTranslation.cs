using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class ServiceTypeTranslation : Entity, IEntityTranslation<ServiceType>
    {
        public string Name { get; set; }

        public ServiceType Core { get; private set; }

        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private ServiceTypeTranslation()
        {

        }

        public ServiceTypeTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            this.Name = name;
            this.CoreId = coreId;
            this.Language = language;
        }
    }
}
