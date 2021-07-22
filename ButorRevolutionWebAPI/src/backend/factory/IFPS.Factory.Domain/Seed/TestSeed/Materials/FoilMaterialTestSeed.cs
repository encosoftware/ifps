using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class FoilMaterialTestSeed : IEntitySeed<FoilMaterial>
    {
        public FoilMaterial[] Entities => new[]
        {
            new FoilMaterial("Foil1",10) { Id = new Guid("0193c159-a13e-4bde-b199-06c885cc7514"),  Description = "Description", Thickness = 2, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000  },
            new FoilMaterial("Foil2", 10) { Id = new Guid("9f1f5d74-e58e-4ea4-9f61-1ef37cf91d4b"), Description = "black&white", Thickness = 4, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000  },
            new FoilMaterial("Foil3", 10) { Id = new Guid("2ecb5a2f-8afa-4a25-b6ff-6de10f52d0e6"), Description = "red&blue", Thickness = 5, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000  },
            new FoilMaterial("Foil4", 10) { Id = new Guid("81ae6bc6-a7b2-45d5-bb2c-a1b174f2e1cb"), Description = "blue&yellow", Thickness = 2, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000  },


            //new FoilMaterial("FL101021WQ", 10) 
            //{ 
            //    Id = new Guid("b2599856-ef0c-491e-9c4f-d0244e90163c"),  
            //    Description = "Festett fehér élzáró fólia", 
            //    Thickness = 2, 
            //    ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), 
            //    CurrentPriceId = 10000,
            //    SiUnitId = 10000
            //},
            //new FoilMaterial("FL0055BLC", 10) 
            //{ 
            //    Id = new Guid("794dbe07-22cd-42fb-8fce-de7ea9d45928"), 
            //    Description = "Festett sötétszürke élzáró fólia", 
            //    Thickness = 4, 
            //    ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), 
            //    CurrentPriceId = 10000,
            //    SiUnitId = 10000
            //},
            //new FoilMaterial("FL002345", 10) 
            //{ 
            //    Id = new Guid("15b3184b-a389-4daf-b562-53d9ea4ad3bf"), 
            //    Description = "Fémes fekete élzáró fólia", 
            //    Thickness = 5, 
            //    ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"),
            //    CurrentPriceId = 10000,
            //    SiUnitId = 10000
            //},
            //new FoilMaterial("FL009995", 10) 
            //{ 
            //    Id = new Guid("4b6fb38f-c242-4693-8614-8b684ce8de45"), 
            //    Description = "Prémium fehért élzáró fólia", 
            //    Thickness = 2, 
            //    ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), 
            //    CurrentPriceId = 10000,
            //    SiUnitId = 10000
            //}
        };
    }
}
