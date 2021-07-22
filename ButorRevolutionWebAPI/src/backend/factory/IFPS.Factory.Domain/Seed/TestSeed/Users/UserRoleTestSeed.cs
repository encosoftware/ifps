using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class UserRoleTestSeed : IEntitySeed<UserRole>
    {
        public UserRole[] Entities => new[]
        {
            new UserRole(userId: 10000, roleId: 10000) {Id = 10000},
            new UserRole(userId: 10001, roleId: 10000) {Id = 10001},
            new UserRole(userId: 10002, roleId: 10000) {Id = 10002},
            new UserRole(userId: 10005, roleId: 10002) {Id = 10003},
            new UserRole(userId: 10006, roleId: 10005) {Id = 10004},
            new UserRole(userId: 10004, roleId: 10000) {Id = 10005},
            new UserRole(userId: 10013, roleId: 10000) {Id = 10006},
            new UserRole(userId: 10003, roleId: 10000) {Id = 10007},
            new UserRole(userId: 10007, roleId: 10000) {Id = 10008},
            new UserRole(userId: 10008, roleId: 10000) {Id = 10009},
            new UserRole(userId: 10009, roleId: 10000) {Id = 10010},
            new UserRole(userId: 10010, roleId: 10000) {Id = 10011},
            new UserRole(userId: 10011, roleId: 10000) {Id = 10012},
            new UserRole(userId: 10012, roleId: 10000) {Id = 10013},
            
        };
    }
}
