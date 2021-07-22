using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class UserInspectionTestSeed : IEntitySeed<UserInspection>
    {
        public UserInspection[] Entities => new[]
        {
            new UserInspection(10000,10000) { Id = 10000 },
            new UserInspection(10000,10001) { Id = 10001 },
            new UserInspection(10000,10002) { Id = 10002 },
            new UserInspection(10000,10003) { Id = 10003 }
        };
    }
}