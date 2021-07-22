using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class UserRoleSeed : IEntitySeed<UserRole>
    {
        public UserRole[] ReleaseEntities => new[]
        {
            new UserRole(userId: 1, roleId: 2) { Id = 1 }
        };

        public UserRole[] Entities => new[]
        {
            new UserRole(userId: 2, roleId: 1) { Id = 2 },
            new UserRole(userId: 3, roleId: 3) { Id = 3 },
            new UserRole(userId: 4, roleId: 5) { Id = 4 },
            new UserRole(userId: 5, roleId: 7) { Id = 5 },
            new UserRole(userId: 6, roleId: 9) { Id = 6 }
        };
    }
}
