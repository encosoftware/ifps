using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class VenueSeed : IEntitySeed<Venue>
    {
        public Venue[] Entities => new[]
        {
            new Venue("Iroda", "+36309736179", "info@encosoftware.hu", null) { Id = 1 },
            new Venue("Székhely", "+36309736179", "info@encosoftware.hu", null) { Id = 2 }
        };
    }
}
