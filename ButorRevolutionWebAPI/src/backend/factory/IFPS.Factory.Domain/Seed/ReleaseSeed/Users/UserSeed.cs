using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class UserSeed : IEntitySeed<User>
    {
        const string defaultPasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==";
        const string defaultSecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR";

        public User[] ReleaseEntities => new[]
        {
            new User("enco@enco.hu") {Id = 1, CurrentVersionId = 1, UserName = "enco", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 }
        };

        public User[] Entities => new[]
        {
            new User("admin@enco.hu") {Id = 2, CurrentVersionId = 2, UserName = "admin", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 },
            new User("production@enco.hu") {Id = 3, CurrentVersionId = 3, UserName = "production", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 },
            new User("financial@enco.hu") {Id = 4, CurrentVersionId = 4, UserName = "financial", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 },
            new User("supply@enco.hu") {Id = 5, CurrentVersionId = 5, UserName = "supply", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 },
            new User("warehouse@enco.hu") {Id = 6, CurrentVersionId = 6, UserName = "warehouse", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), CompanyId = 1 },
            new User("contactperson@contactperson.hu") { Id = 7, CurrentVersionId = 7, UserName = "contact", PasswordHash = defaultPasswordHash, SecurityStamp = defaultSecurityStamp, EmailConfirmed = true, ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523") },
        };
    }
}
