using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class OrderedService : Entity
    {
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        public virtual Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
