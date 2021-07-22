using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class UserSeed : IEntitySeed<User>
    {
        // password: "password"
        const string defaultPasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==";
        const string defaultSecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR";

        public User[] ReleaseEntities => new[]
{
            new User("enco@enco.hu") { Id = 1, CurrentVersionId = 1, UserName = "enco", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 }
        };

        public User[] Entities => new[]
        {
            new User("enco2@enco.hu") { Id = 2, CurrentVersionId = 2, UserName = "enco2", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1},
            new User("enco3@enco.hu") { Id = 3, CurrentVersionId = 3, UserName = "enco3", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1},
            new User("customer@customer.com") { Id = 4, CurrentVersionId = 4, CompanyId = 2, UserName = "CustomerUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") },
            new User("salesperson@salesperson.com") { Id = 5, CurrentVersionId = 5, CompanyId = 1, UserName = "SalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("customertest@customertest.hu") { Id = 6, CurrentVersionId = 6, CompanyId = 2, UserName = "CustomerTest", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") },
            new User("shipping@shipping.hu") { Id = 7, CurrentVersionId = 7, CompanyId = 1, UserName = "ShippingGroup", IsTechnicalAccount = true, PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") },
            new User("morgansalesperson@salesperson.com") { Id = 8, CurrentVersionId = 8, CompanyId = 1, UserName = "PeterSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("stevesales@salesperson.com") { Id = 9, CurrentVersionId = 9, CompanyId = 1, UserName = "SteveSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("johnnysales@salesperson.com") { Id = 10, CurrentVersionId = 10, CompanyId = 1, UserName = "JohnnySalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("ryan@salesperson.com") { Id = 11, CurrentVersionId = 11, CompanyId = 1, UserName = "RyanSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("matthew@salesperson.com") { Id = 12, CurrentVersionId = 12, CompanyId = 1, UserName = "MatthewSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("luke@salesperson.com") { Id = 13, CurrentVersionId = 13, CompanyId = 1, UserName = "LukeSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("eddie@salesperson.com") { Id = 14, CurrentVersionId = 14, CompanyId = 1, UserName = "EddieSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("chuck@salesperson.com") { Id = 15, CurrentVersionId = 15, CompanyId = 1, UserName = "ChuckSalesPersonUser", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523")},
            new User("shipping1@shipping.hu") { Id = 16, CurrentVersionId = 16, CompanyId = 1, UserName = "ShippingUser1", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") },
            new User("shipping2@shipping.hu") { Id = 17, CurrentVersionId = 17, CompanyId = 1, UserName = "ShippingUser2", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") }
        };
    }
}
