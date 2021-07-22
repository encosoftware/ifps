using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class OrderPriceSeed : IEntitySeed<OrderPrice>
    {
        public OrderPrice[] Entities => new[]
        {
            new OrderPrice() { Id = 1 },
            new OrderPrice() { Id = 2 },

            // Statistics page: first and second payments for orders
            new OrderPrice() { Id = 3 },
            new OrderPrice() { Id = 4 },
            new OrderPrice() { Id = 5 },
            new OrderPrice() { Id = 6 },
            new OrderPrice() { Id = 7 },
            new OrderPrice() { Id = 8 },
            new OrderPrice() { Id = 9 },
            new OrderPrice() { Id = 10 },
            new OrderPrice() { Id = 11 },
            new OrderPrice() { Id = 12 },
            new OrderPrice() { Id = 13 },
            new OrderPrice() { Id = 14 },
            new OrderPrice() { Id = 15 },
            new OrderPrice() { Id = 16 },
            new OrderPrice() { Id = 17 },
            new OrderPrice() { Id = 18 }
        };
        //public OrderPrice[] Entities => new OrderPrice[] { };
    }
}
