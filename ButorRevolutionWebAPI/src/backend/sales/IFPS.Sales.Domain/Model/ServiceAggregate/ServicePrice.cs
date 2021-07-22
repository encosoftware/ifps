using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Model.Interfaces;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class ServicePrice : FullAuditedEntity, IEntityPrice<Service>
    {
        /// <summary>
        /// Service
        /// </summary>
        public Service Core { get; set; }
        public int? CoreId { get; set; }

        /// <summary>
        /// Price of the service
        /// </summary>
        public Price Price { get; set; }


        /// <summary>
        /// Date, from which the price is valid
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Date, up to which the price is valid
        /// </summary>
        public DateTime? ValidTo { get; set; }

        public ServicePrice()
        {
            ValidFrom = Clock.Now;
        }

        public ServicePrice(int coreId, Price price) : this()
        {
            CoreId = coreId;
            Price = price;
            ValidFrom = DateTime.Now;
        }

        public void SetValidTo(DateTime? dateTime)
        {
            ValidTo = dateTime;
        }
    }
}
