using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class UserDataTestSeed : IEntitySeed<UserData>
    {
        public UserData[] Entities => new[]
        {
            new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now, null) { Id = 10000 },
            new UserData("Fyodor Dostoevsky", "+297563572", Clock.Now, null) { Id = 10001},
            new UserData("Mihail Bulgakov", "+875345434", Clock.Now, null) { Id = 10002 },
            new UserData("Aldous Huxley", "+55465264534", Clock.Now, null) { Id = 10003 },
            new UserData("George Orwell", "+198419841984", Clock.Now, null) { Id = 10004 },
            new UserData("Para Zita", "06301234567", Clock.Now, null) { Id = 10005 },
            new UserData("Har Mónika", "", Clock.Now, null) { Id = 10006 },
            new UserData("Har Mónika1", "", Clock.Now, null) { Id = 10007 },
            new UserData("Har Mónika2", "", Clock.Now, null) { Id = 10008 },
            new UserData("Har Mónika3", "", Clock.Now, null) { Id = 10009 },
            new UserData("Nemer Eszti2", "06701234567", Clock.Now, null) { Id = 10010 },
            new UserData("Nemer Eszti3", "", Clock.Now, null) { Id = 10011 },

            new UserData("Test Worker Name", "+367012345678", Clock.Now, null) { Id = 10012 }
        };
    }
}