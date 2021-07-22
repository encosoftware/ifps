using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class UserInspectionSeed : IEntitySeed<UserInspection>
    {
        public UserInspection[] Entities => new[]
        {
            new UserInspection(3,1) { Id = 1 },
            new UserInspection(2,2) { Id = 2 },
            new UserInspection(2,3) { Id = 3 },
            new UserInspection(2,4) { Id = 4 }
        };

        //public UserInspection[] Entities => new UserInspection[] { };
    }
}