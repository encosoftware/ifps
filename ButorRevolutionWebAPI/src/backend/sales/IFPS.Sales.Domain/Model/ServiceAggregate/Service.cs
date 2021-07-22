using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Model.Interfaces;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class Service : FullAuditedAggregateRoot, IPricedEntity<ServicePrice>
    {
        public string Description { get; set; }

        public ServiceType ServiceType { get; set; }
        public int ServiceTypeId { get; set; }

        private List<ServicePrice> _prices;
        public IEnumerable<ServicePrice> Prices => _prices.AsReadOnly();

        public int? CurrentPriceId { get; set; }
        public ServicePrice CurrentPrice { get; set; }

        private Service()
        {
            _prices = new List<ServicePrice>();
        }

        public Service(string description) : this()
        {
            Description = description;
        }
    }
}
