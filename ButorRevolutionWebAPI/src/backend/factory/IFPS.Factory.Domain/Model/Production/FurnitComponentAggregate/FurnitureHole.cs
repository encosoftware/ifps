using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
{
    public class FurnitureHole : Entity
    {
        public virtual FurnitureComponent FurnitureComponent { get; set; }
        public int FurnitureComponentId { get; set; }

        public virtual AbsolutePoint StartPoint { get; set; }
        public int StartPointId { get; set; }

        public virtual AbsolutePoint EndPoint { get; set; }
        public int EndPointId { get; set; }

        public double Diameter { get; set; }
    }
}
