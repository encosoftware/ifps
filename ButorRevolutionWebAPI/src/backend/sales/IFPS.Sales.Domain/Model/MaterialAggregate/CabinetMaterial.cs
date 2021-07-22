using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class CabinetMaterial : FullAuditedAggregateRoot
    {
        public double Height { get; set; }
        public double Depth { get; set; }

        public CabinetTypeEnum CabinetType { get; set; }

        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }

        public int OuterMaterialId { get; set; }
        public virtual GroupingCategory OuterMaterial { get; set; }

        public int InnerMaterialId { get; set; }
        public virtual GroupingCategory InnerMaterial { get; set; }
       
        public int BackPanelMaterialId { get; set; }
        public virtual GroupingCategory BackPanelMaterial { get; set; }

        public int DoorMaterialId { get; set; }
        public virtual GroupingCategory DoorMaterial { get; set; }

        public string Description { get; set; }
    }
}
