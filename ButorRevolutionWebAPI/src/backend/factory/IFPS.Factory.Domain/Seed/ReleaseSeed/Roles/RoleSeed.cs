using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class RoleSeed : IEntitySeed<Role>
    {
        public Role[] Entities => new[]
        {
            new Role("Admin", divisionId: 1) { Id = 1 },
            new Role("Admin Expert", divisionId: 1) { Id = 2 },
            new Role("Production", divisionId: 2) { Id = 3 },
            new Role("Financial", divisionId: 3) { Id = 5 },
            new Role("Supply", divisionId: 4) { Id = 7 },
            new Role("Warehouse", divisionId: 5) { Id = 9 },
        };
    }
}
