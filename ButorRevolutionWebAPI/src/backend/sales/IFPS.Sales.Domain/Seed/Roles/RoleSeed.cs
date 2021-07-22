using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class RoleSeed : IEntitySeed<Role>
    {
        public Role[] Entities => new[]
        {
            new Role("Admin", 1) { Id = 1 },
            new Role("Admin Expert", 1) { Id = 2 },
            new Role("Sales", 2) { Id = 3 },
            new Role("Partner", 3) { Id = 4 },
            new Role("Customer", 4) { Id = 5 }
        };
    }
}
