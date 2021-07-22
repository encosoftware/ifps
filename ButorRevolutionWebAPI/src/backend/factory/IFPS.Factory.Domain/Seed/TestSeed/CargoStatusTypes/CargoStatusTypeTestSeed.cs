using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoStatusTypeTestSeed : IEntitySeed<CargoStatusType>
    {
        public CargoStatusType[] Entities => new[]
        {
            new CargoStatusType(CargoStatusEnum.Ordered) { Id = 10000 },
            new CargoStatusType(CargoStatusEnum.WaitingForStocking) { Id = 10001 }
        };
    }
}
