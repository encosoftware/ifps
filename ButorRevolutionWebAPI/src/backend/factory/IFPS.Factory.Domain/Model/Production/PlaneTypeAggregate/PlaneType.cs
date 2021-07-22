using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class PlaneType : AggregateRoot
    {
        public PlaneTypeEnum Type { get; private set; }

        public PlaneType(PlaneTypeEnum type)
        {
            Type = type;
        }
    }
}
