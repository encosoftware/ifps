using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class DecorBoardMaterialTestSeed : IEntitySeed<DecorBoardMaterial>
    {
        public DecorBoardMaterial[] Entities => new[]
        {
            new DecorBoardMaterial("DEC",10) { Id = new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), CategoryId = 10000, Description= "beautiful dec",  HasFiberDirection = false, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10001  },
            new DecorBoardMaterial("DECZ345",8) { Id = new Guid("8b0936ec-0e49-4473-bb1d-2b8bb0564d34"), CategoryId = 10000, Description= "beautiful dec 2",  HasFiberDirection = false, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10001  },
            new DecorBoardMaterial("EXSE", 1) { Id = new Guid("3c3fa639-6451-49d7-b7b2-965092ebccf1"), CategoryId = 10000, Description= "add már, uram, az esőt", HasFiberDirection = false, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10001  },


            new DecorBoardMaterial("BL00712098") 
            { 
                Id = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"), 
                CategoryId = 10000, 
                Description= "Premium fenyő bútorlap", 
                HasFiberDirection = false, 
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"),
                SiUnitId = 10001,
                CurrentPriceId = 10000
            }
        };
    }
}