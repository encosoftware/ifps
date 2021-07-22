using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class ServicePriceSeed : IEntitySeed<ServicePrice>
    {
        public ServicePrice[] Entities => new[]
        {
            new ServicePrice() { Id = 1 },
            new ServicePrice() { Id = 2 },
            new ServicePrice() { Id = 3 },
            new ServicePrice() { Id = 4 },
            new ServicePrice() { Id = 5 },
            new ServicePrice() { Id = 6 },
            new ServicePrice() { Id = 7 },
            new ServicePrice() { Id = 8 }
            //new ServicePrice() { Id = 9 },
            //new ServicePrice() { Id = 10 },
        };
    }
}
