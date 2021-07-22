using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class ConcreteApplianceMaterial : Entity
    {
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        public virtual ApplianceMaterial ApplianceMaterial { get; set; }
        public Guid ApplianceMaterialId { get; set; }

        public int Quantity { get; set; }
    }
}
