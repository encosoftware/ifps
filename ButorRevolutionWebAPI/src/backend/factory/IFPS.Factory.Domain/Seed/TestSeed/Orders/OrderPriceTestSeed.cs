using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderPriceTestSeed : IEntitySeed<OrderPrice>
    {
        public OrderPrice[] Entities => new[]
        {
            new OrderPrice() { Id = 10000 },
            new OrderPrice() { Id = 10001 }
        };
    }
}
