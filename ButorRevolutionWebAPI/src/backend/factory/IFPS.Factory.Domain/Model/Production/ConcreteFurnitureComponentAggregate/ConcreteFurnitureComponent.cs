using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class ConcreteFurnitureComponent : AggregateRoot
    {
        public virtual ConcreteFurnitureUnit ConcreteFurnitureUnit { get; set; }
        public int ConcreteFurnitureUnitId { get; set; }

        public virtual FurnitureComponent FurnitureComponent { get; set; }
        public Guid FurnitureComponentId { get; set; }

        public virtual Cutting Cutting { get; set; }
        public int? CuttingId { get; set; }

        public Image QRCode { get; set; }
        public Guid? QRCodeId { get; set; }

        public CfcStatusEnum Status { get; set; }

        public ConcreteFurnitureComponent(int concreteFurnitureUnitId, Guid furnitureComponentId)
        {
            ConcreteFurnitureUnitId = concreteFurnitureUnitId;
            FurnitureComponentId = furnitureComponentId;
        }

        public ConcreteFurnitureComponent(int concreteFurnitureUnitId)
        {
            ConcreteFurnitureUnitId = concreteFurnitureUnitId;
        }
    }
}
