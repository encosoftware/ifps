using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ServiceSeed : IEntitySeed<Service>
    {
        public Service[] Entities => new[]
        {
            new Service("0 km") { Id = 1, ServiceTypeId = 1, CurrentPriceId = 1 },
            new Service("0 - 19 km") { Id = 2, ServiceTypeId = 1, CurrentPriceId = 2 },
            new Service("20 - 39 km") { Id = 3, ServiceTypeId = 1, CurrentPriceId = 3 },
            new Service("40 - 59 km") { Id = 4, ServiceTypeId = 1, CurrentPriceId = 4 },
            new Service("60 - 79 km") { Id = 5, ServiceTypeId = 1, CurrentPriceId = 5 },
            new Service("60 - 79 km") { Id = 6, ServiceTypeId = 1, CurrentPriceId = 6 },
            new Service("80 - 99 km") { Id = 7, ServiceTypeId = 1, CurrentPriceId = 7 },
            new Service("100 -  km") { Id = 8, ServiceTypeId = 1, CurrentPriceId = 8 },
            new Service("Assembly") { Id = 9, ServiceTypeId = 2/*, CurrentPriceId = 9*/ },
            new Service("Installation") { Id = 10, ServiceTypeId = 3/*, CurrentPriceId = 10*/ }
        };
    }
}
