using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class OrderedFurnitureUnit : Entity
    {
        public virtual Order Order { get; set; }
        public Guid? OrderId { get; set; }

        public virtual WebshopOrder WebshopOrder { get; set; }
        public Guid? WebshopOrderId { get; set; }

        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }

        public virtual Basket Basket { get; set; }
        public int? BasketId { get; set; }

        public Price UnitPrice { get; set; }

        public int Quantity { get; set; }

        private OrderedFurnitureUnit()
        {
            UnitPrice = Price.GetDefaultPrice();
        }

        public OrderedFurnitureUnit(Guid orderId, Guid furnitureUnitId, int quantity, int? basketId = null) : this()
        {
            OrderId = orderId;
            FurnitureUnitId = furnitureUnitId;
            Quantity = quantity;
            BasketId = basketId;
        }

        public OrderedFurnitureUnit(Guid webshopOrderId) : this()
        {
            WebshopOrderId = webshopOrderId;
        }

        public OrderedFurnitureUnit(Guid furnitureUnitId, int quantity, int? basketId = null) : this()
        {
            FurnitureUnitId = furnitureUnitId;
            Quantity = quantity;
            BasketId = basketId;
        }
    }
}
