using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoSeed : IEntitySeed<Cargo>
    {
        public Cargo[] Entities => new[]
        {
            new Cargo("C-3_643_AdS-33 (seed)", 1, 1, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 7, 18), null, null) { Id = 1 },
            new Cargo("B_4_894_GhD_78 (seed)", 2, 2, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 8, 18), null, null) { Id = 2 },
            new Cargo("Z_2_901_RoG_12 (seed)", 3, 1, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 9, 18), null, null) { Id = 3 },
            new Cargo("W_9_007_GSH_02 (seed)", 3, 3, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 10, 18), null, null) { Id = 4 },
            new Cargo("R_2_C3PO_LS_54 (seed)", 3, 2, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 11, 18), null, null) { Id = 5 },
            new Cargo("D_2_MF_PLHS_42 (seed)", 2, 2, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2019, 12, 18), null, null) { Id = 6 },
            new Cargo("N_7_420_Nov_90 (seed)", 1, 3, 1, null, null, null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque quis nisl quis metus faucibus accumsan.", new DateTime(2020, 1, 18), null, null) { Id = 7 }
        };
        //public Cargo[] Entities => new Cargo[] { };
    }
}
