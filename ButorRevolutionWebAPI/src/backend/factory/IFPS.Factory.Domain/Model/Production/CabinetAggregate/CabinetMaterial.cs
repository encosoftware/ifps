using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
{
    public class CabinetMaterial : Entity
    {
        public double Height { get; set; }
        public double Depth { get; set; }

        public int OuterMaterialId { get; set; }
        public virtual BoardMaterial OuterMaterial { get; set; }

        public int InnerMaterialId { get; set; }
        public virtual BoardMaterial InnerMaterial { get; set; }

        public int BackPanelMaterialId { get; set; }
        public virtual BoardMaterial BackPanelMaterial { get; set; }

        public int DoorMaterialId { get; set; }
        public virtual BoardMaterial DoorMaterial { get; set; }

        public string Description { get; set; }
    }
}
