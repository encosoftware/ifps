﻿using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DefaultRoleClaimsSeed : IEntitySeed<DefaultRoleClaim>
    {
        public DefaultRoleClaim[] Entities => new[]
        {
            //Admin
            new DefaultRoleClaim(1,1){ Id = 1},
            new DefaultRoleClaim(1,2){ Id = 2},
            new DefaultRoleClaim(1,3){ Id = 3},
            new DefaultRoleClaim(1,4){ Id = 4},
            new DefaultRoleClaim(1,5){ Id = 5},
            new DefaultRoleClaim(1,6){ Id = 6},
            new DefaultRoleClaim(1,7){ Id = 7},
            new DefaultRoleClaim(1,8){ Id = 8},
            new DefaultRoleClaim(1,9){ Id = 9},
            new DefaultRoleClaim(1,57){ Id = 109},
            new DefaultRoleClaim(1,58){ Id = 110},
            new DefaultRoleClaim(1,59){ Id = 111},

            //Production
            new DefaultRoleClaim(3,10){ Id = 10},
            new DefaultRoleClaim(3,11){ Id = 11},
            new DefaultRoleClaim(3,12){ Id = 12},
            new DefaultRoleClaim(3,13){ Id = 13},
            new DefaultRoleClaim(3,14){ Id = 14},
            new DefaultRoleClaim(3,15){ Id = 15},
            new DefaultRoleClaim(3,16){ Id = 16},
            new DefaultRoleClaim(3,17){ Id = 17},
            new DefaultRoleClaim(3,18){ Id = 18},
            new DefaultRoleClaim(3,19){ Id = 19},
            new DefaultRoleClaim(3,20){ Id = 20},
            new DefaultRoleClaim(3,21){ Id = 21},
            new DefaultRoleClaim(3,22){ Id = 22},
            new DefaultRoleClaim(3,23){ Id = 23},
            new DefaultRoleClaim(3,24){ Id = 24},
            new DefaultRoleClaim(3,60){ Id = 112},
            new DefaultRoleClaim(3,61){ Id = 113},
            new DefaultRoleClaim(3,65){ Id = 117},
            new DefaultRoleClaim(3, 69) { Id = 121 },
            new DefaultRoleClaim(3, 70) { Id = 122 },
            new DefaultRoleClaim(3, 71) { Id = 123 },

            //Financial
            new DefaultRoleClaim(5,25){ Id = 25},
            new DefaultRoleClaim(5,26){ Id = 26},
            new DefaultRoleClaim(5,27){ Id = 27},
            new DefaultRoleClaim(5,66){ Id = 118},
            new DefaultRoleClaim(5,67){ Id = 119},

            //Supply
            new DefaultRoleClaim(7,28){ Id = 28},
            new DefaultRoleClaim(7,29){ Id = 29},
            new DefaultRoleClaim(7,30){ Id = 30},
            new DefaultRoleClaim(7,31){ Id = 31},
            new DefaultRoleClaim(7,32){ Id = 32},

            //Warehouse
            new DefaultRoleClaim(9,33){ Id = 33},
            new DefaultRoleClaim(9,34){ Id = 34},
            new DefaultRoleClaim(9,35){ Id = 35},
            new DefaultRoleClaim(9,36){ Id = 36},
            new DefaultRoleClaim(9,37){ Id = 37},
            new DefaultRoleClaim(9,38){ Id = 38},
            new DefaultRoleClaim(9,39){ Id = 39},
            new DefaultRoleClaim(9,40){ Id = 40},
            new DefaultRoleClaim(9,41){ Id = 41},
            new DefaultRoleClaim(9,42){ Id = 42},
            new DefaultRoleClaim(9,43){ Id = 43},
            new DefaultRoleClaim(9,44){ Id = 44},
            new DefaultRoleClaim(9,68){ Id = 120},

            // Super admin
            new DefaultRoleClaim(2,1){ Id = 45},
            new DefaultRoleClaim(2,2){ Id = 46},
            new DefaultRoleClaim(2,3){ Id = 47},
            new DefaultRoleClaim(2,4){ Id = 48},
            new DefaultRoleClaim(2,5){ Id = 49},
            new DefaultRoleClaim(2,6){ Id = 50},
            new DefaultRoleClaim(2,7){ Id = 51},
            new DefaultRoleClaim(2,8){ Id = 52},
            new DefaultRoleClaim(2,9){ Id = 53},
            new DefaultRoleClaim(2,10){ Id = 54},
            new DefaultRoleClaim(2,11){ Id = 55},
            new DefaultRoleClaim(2,12){ Id = 56},
            new DefaultRoleClaim(2,13){ Id = 57},
            new DefaultRoleClaim(2,14){ Id = 58},
            new DefaultRoleClaim(2,15){ Id = 59},
            new DefaultRoleClaim(2,16){ Id = 60},
            new DefaultRoleClaim(2,17){ Id = 61},
            new DefaultRoleClaim(2,18){ Id = 62},
            new DefaultRoleClaim(2,19){ Id = 63},
            new DefaultRoleClaim(2,20){ Id = 64},
            new DefaultRoleClaim(2,21){ Id = 65},
            new DefaultRoleClaim(2,22){ Id = 66},
            new DefaultRoleClaim(2,23){ Id = 67},
            new DefaultRoleClaim(2,24){ Id = 68},
            new DefaultRoleClaim(2,25){ Id = 69},
            new DefaultRoleClaim(2,26){ Id = 70},
            new DefaultRoleClaim(2,27){ Id = 71},
            new DefaultRoleClaim(2,28){ Id = 72},
            new DefaultRoleClaim(2,29){ Id = 73},
            new DefaultRoleClaim(2,30){ Id = 74},
            new DefaultRoleClaim(2,31){ Id = 75},
            new DefaultRoleClaim(2,32){ Id = 76},
            new DefaultRoleClaim(2,33){ Id = 77},
            new DefaultRoleClaim(2,34){ Id = 78},
            new DefaultRoleClaim(2,35){ Id = 79},
            new DefaultRoleClaim(2,36){ Id = 80},
            new DefaultRoleClaim(2,37){ Id = 81},
            new DefaultRoleClaim(2,38){ Id = 82},
            new DefaultRoleClaim(2,39){ Id = 83},
            new DefaultRoleClaim(2,40){ Id = 84},
            new DefaultRoleClaim(2,41){ Id = 85},
            new DefaultRoleClaim(2,42){ Id = 86},
            new DefaultRoleClaim(2,43){ Id = 87},
            new DefaultRoleClaim(2,44){ Id = 88},
            new DefaultRoleClaim(2,57){ Id = 89},
            new DefaultRoleClaim(2,58){ Id = 90},
            new DefaultRoleClaim(2,59){ Id = 91},
            new DefaultRoleClaim(2,60){ Id = 92},
            new DefaultRoleClaim(2,61){ Id = 93},
            new DefaultRoleClaim(2,65){ Id = 97},
            new DefaultRoleClaim(2,66){ Id = 98},
            new DefaultRoleClaim(2,67){ Id = 99},
            new DefaultRoleClaim(2,68){ Id = 100},
            new DefaultRoleClaim(2, 69) { Id = 101 },
            new DefaultRoleClaim(2, 70) { Id = 102 },
            new DefaultRoleClaim(2, 71) { Id = 103 },
        };
    }
}