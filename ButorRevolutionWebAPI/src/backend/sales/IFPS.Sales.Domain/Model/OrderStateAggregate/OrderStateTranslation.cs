using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class OrderStateTranslation : Entity, IEntityTranslation<OrderState>
    {
        public string Name { get; set; }
        public OrderState Core { get; private set; }

        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        public OrderStateTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            this.Name = name;
            this.CoreId = coreId;
            this.Language = language;
        }
    }
}
