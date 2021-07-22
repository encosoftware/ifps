using ENCO.DDD.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Model
{
    public class OrderedApplianceMaterial : Entity
    {
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        public virtual ApplianceMaterial ApplianceMaterial { get; set; }
        public Guid ApplianceMaterialId { get; set; }

        public int Quantity { get; set; }
    }
}
