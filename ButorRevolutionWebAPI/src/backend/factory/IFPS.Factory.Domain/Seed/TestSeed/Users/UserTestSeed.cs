using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class UserTestSeed : IEntitySeed<User>
    {
        const string defaultPasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==";
        const string defaultSecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR";

        public User[] Entities => new[]
        {
            new User("enco@enco.hu") { Id = 10000, CurrentVersionId = 10000, CreationTime = new DateTime(2018, 05, 10), UserName = "enco", CompanyId = 10000, PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId =new Guid("2154e885-c52f-4e70-9408-f919db51fdae") },
            new User("zelda@göttingsberg.com", LanguageTypeEnum.EN) { Id = 10001, CurrentVersionId = 10000, CreationTime = new DateTime(2018, 05, 10), CompanyId = 10000 },
            new User("salesperson@test.hu", LanguageTypeEnum.HU) { Id = 10002, CurrentVersionId = 10001, CreationTime = new DateTime(2018, 05, 10) },
            new User("customer@test.com", LanguageTypeEnum.EN) { Id = 10003, CurrentVersionId = 10001, CreationTime = new DateTime(2018, 05, 10) },
            new User("contact@enco.test.hu", LanguageTypeEnum.HU) { Id = 10004, CurrentVersionId = 10002, CreationTime = new DateTime(2019, 05, 10) },
            new User("aldous@huxley.org", LanguageTypeEnum.HU) { Id = 10005, CurrentVersionId = 10003, CreationTime = new DateTime(2018, 05, 10) },
            new User("george@enco.test.hu", LanguageTypeEnum.EN) { Id = 10006, CurrentVersionId = 10004, CompanyId = 10004, CreationTime = new DateTime(2019, 05, 10) },
            new User("pa@enco.hu") { Id = 10007, CurrentVersionId = 10005, CreationTime = new DateTime(2019, 03, 01), CompanyId = 10002 },
            new User("hm@enco.hu") { Id = 10008, CurrentVersionId = 10006, CreationTime = new DateTime(2019, 03, 01) },
            new User("hm1@enco.hu") { Id = 10009, CurrentVersionId = 10007, CreationTime = new DateTime(2019, 03, 01), },
            new User("hm2@enco.hu") { Id = 10010, CurrentVersionId = 10008, CreationTime = new DateTime(2019, 03, 01), CompanyId = 10003 },
            new User("hm3@enco.hu") { Id = 10011, CurrentVersionId = 10009, CreationTime = new DateTime(2019, 03, 01) },
            new User("ne@enco.hu") { Id = 10012, CurrentVersionId = 10010, CreationTime = new DateTime(2019, 06, 05), CompanyId = 10003 },
            new User("ne3@enco.hu") { Id = 10013, CurrentVersionId = 10011, CreationTime = new DateTime(2019, 06, 05)},

            new User("processworker@enco.hu", LanguageTypeEnum.HU) { Id = 10014, CurrentVersionId = 10012 }
        };
    }
}