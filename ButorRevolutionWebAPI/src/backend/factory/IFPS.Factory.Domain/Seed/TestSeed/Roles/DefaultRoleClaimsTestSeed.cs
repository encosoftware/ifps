using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DefaultRoleClaimsTestSeed : IEntitySeed<DefaultRoleClaim>
    {
        public DefaultRoleClaim[] Entities => new[]
        {
            new DefaultRoleClaim(10000,10000){ Id = 10000},
            new DefaultRoleClaim(10001,10000){ Id = 10001},
            new DefaultRoleClaim(10001,10001){ Id = 10002},
            new DefaultRoleClaim(10001,10002){ Id = 10003},
            new DefaultRoleClaim(10001,10003){ Id = 10004},
            new DefaultRoleClaim(10002,10004){ Id = 10005},
            new DefaultRoleClaim(10003,10004){ Id = 10006},
            new DefaultRoleClaim(10003,10005){ Id = 10007},
            new DefaultRoleClaim(10003,10006){ Id = 10008},
            new DefaultRoleClaim(10003,10007){ Id = 10009},
            new DefaultRoleClaim(10004,10008){ Id = 10010},
            new DefaultRoleClaim(10005,10008){ Id = 10011},
            new DefaultRoleClaim(10006,10010){ Id = 10012}
        };
    }
}
