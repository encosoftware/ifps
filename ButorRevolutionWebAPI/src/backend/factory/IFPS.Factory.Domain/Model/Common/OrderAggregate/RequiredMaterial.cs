using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class RequiredMaterial : FullAuditedAggregateRoot
    {
        public Order Order { get; set; }
        public Guid OrderId { get; set; }

        public Material Material { get; set; }
        public Guid MaterialId { get; set; }

        public int RequiredAmount { get; set; }

        private RequiredMaterial() { }

        public RequiredMaterial(Guid orderId, Guid materialId, int requiredAmount)
        {
            OrderId = orderId;
            MaterialId = materialId;
            RequiredAmount = requiredAmount;            
        }
    }
}
