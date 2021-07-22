using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
{
    public class Cutting : Entity
    {
        public int TopLeftX { get; set; }
        public int TopLeftY { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public virtual LayoutPlan LayoutPlan { get; set; }
        public int LayoutPlanId { get; set; }

        public Cutting(int layoutPlanId)
        {
            LayoutPlanId = layoutPlanId;
        }
    }
}
