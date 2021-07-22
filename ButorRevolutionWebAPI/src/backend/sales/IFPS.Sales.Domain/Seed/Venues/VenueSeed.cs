using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Venues
{
    public class VenueSeed : IEntitySeed<Venue>
    {
        public Venue[] Entities => new[]
        {
            new Venue("Iroda (seed)", "", "test@test.com", 1, null) { Id = 1},
            new Venue("Székhely (seed)", "", "test@test.com",1, null) { Id = 2}
        };
    }
}
