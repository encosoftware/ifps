using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class UserTeamSeed : IEntitySeed<UserTeam>
    {
        public UserTeam[] Entities => new[]
        {
            new UserTeam(1, 7, 16, 1) { Id = 1 }, //3
            new UserTeam(1, 7, 17, 1) { Id = 2 } //4
        };
    }
}
