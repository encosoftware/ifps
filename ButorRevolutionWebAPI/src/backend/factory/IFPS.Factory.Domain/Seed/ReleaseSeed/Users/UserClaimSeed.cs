using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class UserClaimSeed : IEntitySeed<UserClaim>
    {
        public UserClaim[] ReleaseEntities => new[]
        {
            //Super admin user
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
            new UserClaim(1, 32) { Id = 32 },
            new UserClaim(1, 33) { Id = 33 },
            new UserClaim(1, 34) { Id = 34 },
            new UserClaim(1, 35) { Id = 35 },
            new UserClaim(1, 36) { Id = 36 },
            new UserClaim(1, 37) { Id = 37 },
            new UserClaim(1, 38) { Id = 38 },
            new UserClaim(1, 39) { Id = 39 },
            new UserClaim(1, 40) { Id = 40 },
            new UserClaim(1, 41) { Id = 41 },
            new UserClaim(1, 42) { Id = 42 },
            new UserClaim(1, 43) { Id = 43 },
            new UserClaim(1, 44) { Id = 44 },
            new UserClaim(1, 57) { Id = 110 },
            new UserClaim(1, 58) { Id = 111 },
            new UserClaim(1, 59) { Id = 112 },
            new UserClaim(1, 60) { Id = 113 },
            new UserClaim(1, 61) { Id = 114 },
            new UserClaim(1, 65) { Id = 115 },
            new UserClaim(1, 66) { Id = 116 },
            new UserClaim(1, 67) { Id = 117 },
            new UserClaim(1, 68) { Id = 118 },
            new UserClaim(1, 69) { Id = 128 },
            new UserClaim(1, 70) { Id = 129 },
            new UserClaim(1, 71) { Id = 130 },
            new UserClaim(1, 72) { Id = 134 }
        };

        public UserClaim[] Entities => new[]
        {
            //Admin user
            new UserClaim(2, 1) { Id = 45 },
            new UserClaim(2, 2) { Id = 46 },
            new UserClaim(2, 3) { Id = 47 },
            new UserClaim(2, 4) { Id = 48 },
            new UserClaim(2, 5) { Id = 49 },
            new UserClaim(2, 6) { Id = 50 },
            new UserClaim(2, 7) { Id = 51 },
            new UserClaim(2, 8) { Id = 52 },
            new UserClaim(2, 9) { Id = 53 },
            new UserClaim(2, 57) { Id = 119 },
            new UserClaim(2, 58) { Id = 120 },
            new UserClaim(2, 59) { Id = 121 },

            //Production user
            new UserClaim(3, 10) { Id = 54 },
            new UserClaim(3, 11) { Id = 55 },
            new UserClaim(3, 12) { Id = 56 },
            new UserClaim(3, 13) { Id = 57 },
            new UserClaim(3, 14) { Id = 58 },
            new UserClaim(3, 15) { Id = 59 },
            new UserClaim(3, 16) { Id = 60 },
            new UserClaim(3, 17) { Id = 61 },
            new UserClaim(3, 18) { Id = 62 },
            new UserClaim(3, 19) { Id = 63 },
            new UserClaim(3, 20) { Id = 64 },
            new UserClaim(3, 21) { Id = 65 },
            new UserClaim(3, 22) { Id = 66 },
            new UserClaim(3, 23) { Id = 67 },
            new UserClaim(3, 24) { Id = 68 },
            new UserClaim(3, 60) { Id = 122 },
            new UserClaim(3, 61) { Id = 123 },
            new UserClaim(3, 65) { Id = 124 },
            new UserClaim(3, 69) { Id = 131 },
            new UserClaim(3, 70) { Id = 132 },
            new UserClaim(3, 71) { Id = 133 },
            new UserClaim(1, 72) { Id = 135 },

            //Financial user
            new UserClaim(4, 25) { Id = 69 },
            new UserClaim(4, 26) { Id = 70 },
            new UserClaim(4, 27) { Id = 71 },
            new UserClaim(4, 66) { Id = 125 },
            new UserClaim(4, 67) { Id = 126 },

            //Supply user
            new UserClaim(5, 28) { Id = 72 },
            new UserClaim(5, 29) { Id = 73 },
            new UserClaim(5, 30) { Id = 74 },
            new UserClaim(5, 31) { Id = 75 },
            new UserClaim(5, 32) { Id = 76 },

            //Warehouse user
            new UserClaim(6, 33) { Id = 77 },
            new UserClaim(6, 34) { Id = 78 },
            new UserClaim(6, 35) { Id = 79 },
            new UserClaim(6, 36) { Id = 80 },
            new UserClaim(6, 37) { Id = 81 },
            new UserClaim(6, 38) { Id = 82 },
            new UserClaim(6, 39) { Id = 83 },
            new UserClaim(6, 40) { Id = 84 },
            new UserClaim(6, 41) { Id = 85 },
            new UserClaim(6, 42) { Id = 86 },
            new UserClaim(6, 43) { Id = 87 },
            new UserClaim(6, 44) { Id = 88 },
            new UserClaim(6, 68) { Id = 127 }
        };
    }
}