using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class UserDataSeed : IEntitySeed<UserData>
    {
        public UserData[] ReleaseEntities => new[]
        {
            new UserData("SUPER ADMIN","06205555555",Clock.Now) { Id = 1 }
        };

        public UserData[] Entities => new[]
        {
            new UserData("Admin 1 (seed)","06205555555",Clock.Now) { Id = 2 },
            new UserData("Admin 2 (seed)","06205555555",Clock.Now) { Id = 3 },
            new UserData("Tommy Customer (seed)","062562565",Clock.Now) { Id = 4 },
            new UserData("Freddy SalesPerson (seed)","0645645657",Clock.Now) { Id = 5 },
            new UserData("Fred Customer (seed)","0641244587",Clock.Now) { Id = 6 },
            new UserData("Shipping Team","", Clock.Now) { Id = 7 },
            new UserData("Morgan SalesPerson (seed)","0643645657",Clock.Now) { Id = 8 },
            new UserData("Steve SalesPerson (seed)","0645445657",Clock.Now) { Id = 9 },
            new UserData("Johnny SalesPerson (seed)","0645645657",Clock.Now) { Id = 10 },
            new UserData("Ryan SalesPerson (seed)","0645665657",Clock.Now) { Id = 11 },
            new UserData("Matthew SalesPerson (seed)","0675645657",Clock.Now) { Id = 12 },
            new UserData("Luke SalesPerson (seed)","0645685657",Clock.Now) { Id = 13 },
            new UserData("Eddie SalesPerson (seed)","0645945657",Clock.Now) { Id = 14 },
            new UserData("Chuck SalesPerson (seed)","0645145657",Clock.Now) { Id = 15 },
            new UserData("Chuck Shipping (seed)","0645145657",Clock.Now) { Id = 16 },
            new UserData("Luke Shipping (seed)","0645145657",Clock.Now) { Id = 17 },
        };
    }
}
