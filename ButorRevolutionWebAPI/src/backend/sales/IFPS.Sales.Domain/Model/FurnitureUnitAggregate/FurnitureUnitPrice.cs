using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Model.Interfaces;
using System;


namespace IFPS.Sales.Domain.Model
{
    public class FurnitureUnitPrice : FullAuditedEntity, IEntityPrice<FurnitureUnit, Guid>
    {
        /// <summary>
        /// Furniture unit
        /// </summary>
        public FurnitureUnit Core { get; set; }
        public Guid? CoreId { get; set; }

        /// <summary>
        /// Price of the furniture unit
        /// </summary>
        public Price Price { get; set; }


        /// <summary>
        /// Cost of the material
        /// </summary>
        public Price MaterialCost { get; set; }


        /// <summary>
        /// Date, from which the price is valid
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Date, up to which the price is valid
        /// </summary>
        public DateTime? ValidTo { get; set; }

        public FurnitureUnitPrice()
        {

        }

        public FurnitureUnitPrice(Guid coreId, Price price)
        {
            CoreId = coreId;
            Price = price;
            ValidFrom = DateTime.Now;
        }

        public void SetValidTo(DateTime? dateTime) => ValidTo = dateTime;
    }
}
