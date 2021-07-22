using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class Basket : FullAuditedAggregateRoot
    {
        public virtual Customer Customer { get; set; }
        public int? CustomerId { get; set; }

        private List<OrderedFurnitureUnit> _orderedFurnitureUnits;
        public IEnumerable<OrderedFurnitureUnit> OrderedFurnitureUnits => _orderedFurnitureUnits.AsReadOnly();

        public virtual Service Service { get; set; }
        public int ServiceId { get; set; }

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string TaxNumber { get; set; }
        public string Note { get; set; }

        public Price SubTotal { get; set; }
        public Price DelieveryPrice { get; set; }

        public Address BillingAddress { get; set; }
        public Address DelieveryAddress { get; set; }

        public bool GaveEmailConsent { get; set; }

        public Email Email { get; set; }
        public int? EmailId { get; set; }

        private Basket()
        {
            _orderedFurnitureUnits = new List<OrderedFurnitureUnit>();
            CreationTime = Clock.Now;
            ServiceId = 1;
        }

        public Basket(int? customerId = null) : this()
        {
            CustomerId = customerId;
            DelieveryPrice = Price.GetDefaultPrice();
            BillingAddress = Address.GetEmptyAddress();
            DelieveryAddress = Address.GetEmptyAddress();
        }

        public void AddOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Add(orderedFurnitureUnit);
        }

        public void RemoveOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Remove(orderedFurnitureUnit);
        }

        public void ClearOrderedFurnitureUnits()
        {
            _orderedFurnitureUnits.Clear();
        }

        public void ClearSubtotal()
        {
            SubTotal = Price.GetDefaultPrice();
        }

        public void SetSubTotalAfterDeleteItem(Price price, OrderedFurnitureUnit orderedFurnitureUnit)
        {
            SubTotal.Value -= price.Value * orderedFurnitureUnit.Quantity;
        }
    }
}
