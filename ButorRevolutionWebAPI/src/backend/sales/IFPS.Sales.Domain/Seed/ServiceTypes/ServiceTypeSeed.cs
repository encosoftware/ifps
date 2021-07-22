using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ServiceTypeSeed : IEntitySeed<ServiceType>
    {
        public ServiceType[] Entities => new[]
        {
            new ServiceType(ServiceTypeEnum.Shipping) { Id = 1 },
            new ServiceType(ServiceTypeEnum.Assembly) { Id = 2 },
            new ServiceType(ServiceTypeEnum.Installation) { Id = 3 }
        };
    }
}
