using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class UserClaimSeed : IEntitySeed<UserClaim>
    {
        public UserClaim[] ReleaseEntities => new[]
        {
            //ADMIN EXPERT
            new UserClaim(1, 1) { Id = 1 },
            new UserClaim(1, 2) { Id = 2 },
            new UserClaim(1, 3) { Id = 3 },
            new UserClaim(1, 4) { Id = 4 },
            new UserClaim(1, 5) { Id = 5 },
            new UserClaim(1, 6) { Id = 6 },
            new UserClaim(1, 7) { Id = 7 },
            new UserClaim(1, 8) { Id = 8 },
            new UserClaim(1, 9) { Id = 9 },
            new UserClaim(1, 10) { Id = 10 },
            new UserClaim(1, 11) { Id = 11 },
            new UserClaim(1, 12) { Id = 12 },
            new UserClaim(1, 13) { Id = 13 },
            new UserClaim(1, 14) { Id = 14 },
            new UserClaim(1, 15) { Id = 15 },
            new UserClaim(1, 16) { Id = 16 },
            new UserClaim(1, 17) { Id = 17 },
            new UserClaim(1, 18) { Id = 18 },
            new UserClaim(1, 19) { Id = 19 },
            new UserClaim(1, 20) { Id = 20 },
            new UserClaim(1, 21) { Id = 21 },
            new UserClaim(1, 22) { Id = 22 },
            new UserClaim(1, 23) { Id = 23 },
            new UserClaim(1, 24) { Id = 24 },
            new UserClaim(1, 25) { Id = 25 },
            new UserClaim(1, 26) { Id = 26 },
            new UserClaim(1, 27) { Id = 27 },
            new UserClaim(1, 28) { Id = 28 },
            new UserClaim(1, 29) { Id = 29 },
            new UserClaim(1, 30) { Id = 30 },
            new UserClaim(1, 31) { Id = 31 },
            new UserClaim(1, 33) { Id = 32 },
            new UserClaim(1, 34) { Id = 33 },
            new UserClaim(1, 35) { Id = 34 }
        };

        public UserClaim[] Entities => new[]
        {
            //SALESPERSON
            new UserClaim(5, 22) { Id = 36 },
            new UserClaim(5, 23) { Id = 37 },
            new UserClaim(5, 24) { Id = 38 },
            new UserClaim(5, 25) { Id = 39 },
            new UserClaim(5, 26) { Id = 40 },
            new UserClaim(5, 27) { Id = 41 },
            new UserClaim(5, 28) { Id = 42 },
            new UserClaim(5, 29) { Id = 43 },
            new UserClaim(5, 30) { Id = 44 },
            new UserClaim(5, 31) { Id = 45 },
            new UserClaim(5, 32) { Id = 46 },
            new UserClaim(5, 33) { Id = 47 },
            new UserClaim(5, 34) { Id = 48 },
            new UserClaim(5, 35) { Id = 49 },

            //PARTNER
            new UserClaim(7, 28) { Id = 50 },
            new UserClaim(7, 29) { Id = 51 },
            new UserClaim(7, 30) { Id = 52 },
            new UserClaim(7, 31) { Id = 53 },
            new UserClaim(7, 33) { Id = 54 },
            new UserClaim(7, 36) { Id = 55 },
            new UserClaim(7, 37) { Id = 56 },
            new UserClaim(7, 38) { Id = 57 },

            new UserClaim(16, 28) { Id = 58 },
            new UserClaim(16, 29) { Id = 59 },
            new UserClaim(16, 30) { Id = 60 },
            new UserClaim(16, 31) { Id = 61 },
            new UserClaim(16, 33) { Id = 62 },
            new UserClaim(16, 36) { Id = 63 },
            new UserClaim(16, 37) { Id = 64 },
            new UserClaim(16, 38) { Id = 65 },

            new UserClaim(17, 28) { Id = 66 },
            new UserClaim(17, 29) { Id = 67 },
            new UserClaim(17, 30) { Id = 68 },
            new UserClaim(17, 31) { Id = 69 },
            new UserClaim(17, 33) { Id = 70 },
            new UserClaim(17, 36) { Id = 71 },
            new UserClaim(17, 37) { Id = 72 },
            new UserClaim(17, 38) { Id = 73 },

            //CUSTOMER
            new UserClaim(4, 28) { Id = 74 },
            new UserClaim(4, 30) { Id = 75 },
            new UserClaim(4, 31) { Id = 76 },
            new UserClaim(4, 33) { Id = 77 },
            new UserClaim(4, 39) { Id = 78 },
            new UserClaim(4, 40) { Id = 79 },
            new UserClaim(4, 41) { Id = 80 },
            new UserClaim(4, 29) { Id = 81 }
        };
    }
}
