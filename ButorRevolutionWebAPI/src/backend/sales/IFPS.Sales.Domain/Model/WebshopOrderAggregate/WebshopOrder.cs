using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Events.OrderEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class WebshopOrder : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the order, wich helps the salesperson to identify it. 
        /// By default, it is the AddressValue field of the installment place
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// Unique value, to easily identify the exact order by the year and the serial number
        /// </summary>
        private static readonly string WorkingNumberPrefix = "MSZ";
        public int WorkingNumberYear { get; set; }
        public int WorkingNumberSerial { get; set; }
        public string WorkingNumber => WorkingNumberYear > 0 && WorkingNumberSerial > 0 ?
            WorkingNumberPrefix + WorkingNumberSerial.ToString().PadLeft(4, '0') + '/' + WorkingNumberYear :
            String.Empty;

        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Customer, who requested the 
        /// </summary>
        public virtual Customer Customer { get; set; }
        public int? CustomerId { get; set; }

        /// <summary>
        /// The basket that contains the WebshopOrders
        /// </summary>
        public virtual Basket Basket { get; set; }
        public int BasketId { get; set; }

        private List<OrderedFurnitureUnit> _orderedFurnitureUnits;
        public IEnumerable<OrderedFurnitureUnit> OrderedFurnitureUnits => _orderedFurnitureUnits.AsReadOnly();

        /// <summary>
        /// Private list of services
        /// </summary>
        private List<WebshopOrderService> _services;
        /// <summary>
        /// Public readonly collection of services
        /// </summary>
        public IEnumerable<WebshopOrderService> Services => _services.AsReadOnly();

        private WebshopOrder()
        {
            Id = Guid.NewGuid();
            _orderedFurnitureUnits = new List<OrderedFurnitureUnit>();
            _services = new List<WebshopOrderService>();
            CreationTime = DateTime.Now;
        }


        public WebshopOrder(string orderName, int? customerId, Address shippingAddress) : this()
        {
            OrderName = orderName;
            CustomerId = customerId;
            ShippingAddress = shippingAddress;

            AddDomainEvent(new WebshopOrderPlacedDomainEvent(this));
        }

        public void AddOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Add(orderedFurnitureUnit);
        }

        public void AddOrderedFurnitureUnits(List<OrderedFurnitureUnit> orderedFurnitureUnits)
        {
            orderedFurnitureUnits.ForEach(ent => ent.WebshopOrderId = Id);
            _orderedFurnitureUnits.AddRange(orderedFurnitureUnits);
        }

        public void RemoveOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Remove(orderedFurnitureUnit);
        }

        public void AddService(WebshopOrderService orderedService)
        {
            Ensure.NotNull(orderedService);
            _services.Add(orderedService);
        }

        public void ClearServicesList()
        {
            _services.Clear();
        }

        public void SetWorkingNumber(int year, int serial)
        {
            if (WorkingNumberYear == 0 && WorkingNumberSerial == 0)
            {
                WorkingNumberYear = year;
                WorkingNumberSerial = serial;
            }
            else
            {
                //Test seed conflict 
                //throw new IFPSDomainException("WorkingNumber has already set!");
            }
        }
    }
}
