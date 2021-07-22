using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class DefaultRoleClaimsSeed : IEntitySeed<DefaultRoleClaim>
    {
        public DefaultRoleClaim[] Entities => new[]
        {
            #region ADMIN roles
            new DefaultRoleClaim(1, 1) { Id = 1 },
            new DefaultRoleClaim(1, 2) { Id = 2 },
            new DefaultRoleClaim(1, 3) { Id = 3 },
            new DefaultRoleClaim(1, 4) { Id = 4 },
            new DefaultRoleClaim(1, 5) { Id = 5 },
            new DefaultRoleClaim(1, 6) { Id = 6 },
            new DefaultRoleClaim(1, 7) { Id = 7 },
            new DefaultRoleClaim(1, 8) { Id = 8 },
            new DefaultRoleClaim(1, 9) { Id = 9 },
            new DefaultRoleClaim(1, 10) { Id = 10 },
            new DefaultRoleClaim(1, 11) { Id = 11 },
            new DefaultRoleClaim(1, 12) { Id = 12 },
            new DefaultRoleClaim(1, 13) { Id = 13 },
            new DefaultRoleClaim(1, 14) { Id = 14 },
            new DefaultRoleClaim(1, 15) { Id = 15 },
            new DefaultRoleClaim(1, 16) { Id = 16 },
            new DefaultRoleClaim(1, 17) { Id = 17 },
            new DefaultRoleClaim(1, 18) { Id = 18 },
            new DefaultRoleClaim(1, 19) { Id = 19 },
            new DefaultRoleClaim(1, 20) { Id = 20 },
            new DefaultRoleClaim(1, 21) { Id = 21 },
            #endregion

            #region ADMIN EXPERT roles
            new DefaultRoleClaim(2, 1) { Id = 22 },
            new DefaultRoleClaim(2, 2) { Id = 23 },
            new DefaultRoleClaim(2, 3) { Id = 24 },
            new DefaultRoleClaim(2, 4) { Id = 25 },
            new DefaultRoleClaim(2, 5) { Id = 26 },
            new DefaultRoleClaim(2, 6) { Id = 27 },
            new DefaultRoleClaim(2, 7) { Id = 28 },
            new DefaultRoleClaim(2, 8) { Id = 29 },
            new DefaultRoleClaim(2, 9) { Id = 30 },
            new DefaultRoleClaim(2, 10) { Id = 31 },
            new DefaultRoleClaim(2, 11) { Id = 32 },
            new DefaultRoleClaim(2, 12) { Id = 33 },
            new DefaultRoleClaim(2, 13) { Id = 34 },
            new DefaultRoleClaim(2, 14) { Id = 35 },
            new DefaultRoleClaim(2, 15) { Id = 36 },
            new DefaultRoleClaim(2, 16) { Id = 37 },
            new DefaultRoleClaim(2, 17) { Id = 38 },
            new DefaultRoleClaim(2, 18) { Id = 39 },
            new DefaultRoleClaim(2, 19) { Id = 40 },
            new DefaultRoleClaim(2, 20) { Id = 41 },
            new DefaultRoleClaim(2, 21) { Id = 42 },
            new DefaultRoleClaim(2, 22) { Id = 43 },
            new DefaultRoleClaim(2, 23) { Id = 44 },
            new DefaultRoleClaim(2, 24) { Id = 45 },
            new DefaultRoleClaim(2, 25) { Id = 46 },
            new DefaultRoleClaim(2, 26) { Id = 47 },
            new DefaultRoleClaim(2, 27) { Id = 48 },
            new DefaultRoleClaim(2, 28) { Id = 49 },
            new DefaultRoleClaim(2, 29) { Id = 50 },
            new DefaultRoleClaim(2, 30) { Id = 51 },
            new DefaultRoleClaim(2, 31) { Id = 52 },
            new DefaultRoleClaim(2, 33) { Id = 53 },
            new DefaultRoleClaim(2, 34) { Id = 54 },
            #endregion

            #region SALES roles
            new DefaultRoleClaim(3, 22) { Id = 56 },
            new DefaultRoleClaim(3, 23) { Id = 57 },
            new DefaultRoleClaim(3, 24) { Id = 58 },
            new DefaultRoleClaim(3, 25) { Id = 59 },
            new DefaultRoleClaim(3, 26) { Id = 60 },
            new DefaultRoleClaim(3, 27) { Id = 61 },
            new DefaultRoleClaim(3, 28) { Id = 62 },
            new DefaultRoleClaim(3, 29) { Id = 63 },
            new DefaultRoleClaim(3, 30) { Id = 64 },
            new DefaultRoleClaim(3, 31) { Id = 65 },
            new DefaultRoleClaim(3, 32) { Id = 66 },
            new DefaultRoleClaim(3, 33) { Id = 67 },
            new DefaultRoleClaim(3, 34) { Id = 68 },
            new DefaultRoleClaim(3, 35) { Id = 69 },
            #endregion

            #region PARTNER roles
            new DefaultRoleClaim(4, 28) { Id = 70 },
            new DefaultRoleClaim(4, 29) { Id = 71 },
            new DefaultRoleClaim(4, 30) { Id = 72 },
            new DefaultRoleClaim(4, 31) { Id = 73 },
            new DefaultRoleClaim(4, 33) { Id = 74 },
            new DefaultRoleClaim(4, 36) { Id = 75 },
            new DefaultRoleClaim(4, 37) { Id = 76 },
            new DefaultRoleClaim(4, 38) { Id = 77 },
            #endregion

            #region CUSTOMER roles
            new DefaultRoleClaim(5, 28) { Id = 78 },
            new DefaultRoleClaim(5, 30) { Id = 79 },
            new DefaultRoleClaim(5, 31) { Id = 80 },
            new DefaultRoleClaim(5, 33) { Id = 81 },
            new DefaultRoleClaim(5, 39) { Id = 82 },
            new DefaultRoleClaim(5, 40) { Id = 83 },
            new DefaultRoleClaim(5, 41) { Id = 84 },
            new DefaultRoleClaim(5, 29) { Id = 85 }
            #endregion
        };
    }
}
