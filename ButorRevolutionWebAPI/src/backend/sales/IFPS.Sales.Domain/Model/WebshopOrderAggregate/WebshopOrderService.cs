using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class WebshopOrderService : Entity
    {
        public virtual WebshopOrder WebshopOrder { get; set; }
        public Guid WebshopOrderId { get; set; }

        public virtual Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
