using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoStatusTypeSeed : IEntitySeed<CargoStatusType>
    {
        public CargoStatusType[] Entities => new[]
        {
            new CargoStatusType(CargoStatusEnum.Ordered) { Id = 1 },
            new CargoStatusType(CargoStatusEnum.WaitingForStocking) { Id = 2 },
            new CargoStatusType(CargoStatusEnum.Stocked) { Id = 3 },
        };
    }
}
