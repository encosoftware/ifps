using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class RoleTestSeed : IEntitySeed<Role>
    {
        public Role[] Entities => new[]
        {
            new Role("Admin", divisionId: 10000) {Id = 10000},
            new Role("Admin Expert", divisionId: 10000) {Id = 10001},
            new Role("Production", divisionId: 10001) {Id = 10002},
            new Role("Production Expert", divisionId: 10001) {Id = 10003},
            new Role("Financial", divisionId: 10002) {Id = 10004},
            new Role("Financial Expert", divisionId: 10002) {Id = 10005},
            new Role("Supply", divisionId: 10003) {Id = 10006},
            new Role("Supply Expert", divisionId: 10003) {Id = 10007},
            new Role("Warehouse", divisionId: 10004) {Id = 10008},
            new Role("Warehouse Expert", divisionId: 10004) {Id = 10009}
        };
    }
}
