using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class CustomerFurnitureUnit : FullAuditedEntity
    {
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public WebshopFurnitureUnit WebshopFurnitureUnit { get; set; }
        public int WebshopFurnitureUnitId { get; set; }

        public CustomerFurnitureUnit() { }

        public CustomerFurnitureUnit(int customerId, int webshopFurnitureUnitId)
        {
            CustomerId = customerId;
            WebshopFurnitureUnitId = webshopFurnitureUnitId;
        }
    }
}
