using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class StockedMaterial : FullAuditedAggregateRoot
    {
        public virtual Material Material { get; set; }
        public Guid MaterialId { get; set; }
        
        /// <summary>
        /// Amount available at the warehouse
        /// </summary>
        public double StockedAmount { get; set; } //TODO int?

        /// <summary>
        /// Minimum amount of material in the warehouse
        /// </summary>
        public int MinAmount { get; set; }

        /// <summary>
        /// Required amount of material in the warehouse
        /// </summary>
        public int RequiredAmount { get; set; }

        /// <summary>
        /// Already ordered amount which is not yet delivered to the warehouse
        /// </summary>
        public int OrderedAmount { get; set; }

        private StockedMaterial()
        {

        }

        public StockedMaterial(Guid materialId, double amount, int minAmount, int reqAmount)
        {
            MaterialId = materialId;
            StockedAmount = amount;
            MinAmount = minAmount;
            RequiredAmount = reqAmount;
            OrderedAmount = 0;
        }
    }
}
