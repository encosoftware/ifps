using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class BasketSeed : IEntitySeed<Basket>
    {
        public Basket[] Entities => new[]
        {
            new Basket() { Id = 1, BillingAddress = null, DelieveryAddress = null, DelieveryPrice = null, SubTotal = null }
        };
    }
}
