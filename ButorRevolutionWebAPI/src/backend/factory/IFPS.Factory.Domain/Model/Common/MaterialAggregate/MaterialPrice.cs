using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Model.Interfaces;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class MaterialPrice : FullAuditedEntity, IEntityPrice<Material, Guid>
    {
        /// <summary>
        /// Material type
        /// </summary>
        public Material Core { get; set; }

        public Guid? CoreId { get; set; }

        /// <summary>
        /// Price of the material type
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

        public MaterialPrice()
        {
        }

        public MaterialPrice(Guid coreId, Price price)
        {
            CoreId = coreId;
            Price = price;
            ValidFrom = DateTime.Now;
        }

        public void SetValidTo(DateTime? dateTime)
        {
            this.ValidTo = dateTime;
        }
    }
}