using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class UserRoleSeed : IEntitySeed<UserRole>
    {
        public UserRole[] ReleaseEntities => new[]
        {
            new UserRole(1, 2) {Id = 1 }
        };

        public UserRole[] Entities => new[]
        {
            new UserRole(2, 1) {Id = 2 },
            new UserRole(3, 1) {Id = 3 },
            new UserRole(4, 5) {Id = 4 },
            new UserRole(5, 3) {Id = 5 },
            new UserRole(6, 5) {Id = 6 },
            new UserRole(7, 4) {Id = 7 },
            new UserRole(8, 3) {Id = 8 },
            new UserRole(9, 3) {Id = 9 },
            new UserRole(10, 3) {Id = 10 },
            new UserRole(11, 3) {Id = 11 },
            new UserRole(12, 3) {Id = 12 },
            new UserRole(13, 3) {Id = 13 },
            new UserRole(14, 3) {Id = 14 },
            new UserRole(15, 3) {Id = 15 },
            new UserRole(16, 4) {Id = 16 },
            new UserRole(17, 4) {Id = 17 }
        };
    }
}
