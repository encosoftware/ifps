using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class UserDataSeed : IEntitySeed<UserData>
    {
        public UserData[] ReleaseEntities => new[]
        {
            new UserData("SUPER ADMIN", "+362497489", Clock.Now, null) { Id = 1 }
        };

        public UserData[] Entities => new[]
        {
            new UserData("Admin Enco (seed)", "+297563572", Clock.Now, null) { Id = 2 },
            new UserData("Production (seed)", "+4985634658", Clock.Now, null) { Id = 3 },
            new UserData("Financial (seed)", "+55465264534", Clock.Now, null) { Id = 4 },
            new UserData("Supply (seed)", "+198419841984", Clock.Now, null) { Id = 5 },
            new UserData("Warehouse (seed)","06205555555",Clock.Now, null) { Id = 6 },
            new UserData("Contact (seed)","06206666666",Clock.Now, null) { Id = 7 }
        };
    }
}
