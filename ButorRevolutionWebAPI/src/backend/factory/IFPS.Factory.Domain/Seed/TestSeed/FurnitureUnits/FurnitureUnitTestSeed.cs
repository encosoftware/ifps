using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class FurnitureUnitTestSeed : IEntitySeed<FurnitureUnit>
    {
        public FurnitureUnit[] Entities => new[]
        {
            new FurnitureUnit("EXSA", 1,1,1) { Id = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = null, Description = "Miracle Desc", CurrentPriceId = 10000 },
            new FurnitureUnit("EXSE", 1,2,3) { Id = new Guid("5db3fb96-1619-4b05-afa2-9485c282db76"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47"), Description = "Brilliant Desc", CurrentPriceId = 10001 },

            new FurnitureUnit("Reservation 02", 600,200,300) 
            { 
                Id = new Guid("f4f37e65-379c-4569-a720-83e4f9ec0e90"), 
                CategoryId = 10000, 
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), 
                BaseFurnitureUnitId = null, 
                Description = "lorem ipsum 02", 
                CurrentPriceId = 10001 
            },
            new FurnitureUnit(code: "EXSA", width: 1, height: 1, depth: 1) { Id = new Guid("7ED9EDE3-C0C7-492C-B236-38E4E47BAA10"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = null, Description = "Miracle Desc", CurrentPriceId = 10000 },
            new FurnitureUnit(code: "EXSE", width: 1, height: 2, depth: 3) { Id = new Guid("31E41E48-C8D2-4AEB-873D-76BC4A7C55E4"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"), Description = "Brilliant Desc", CurrentPriceId = 10001 },
            new FurnitureUnit(code: "EXDC", width: 1, height: 3, depth: 4) { Id = new Guid("DDC51E74-B8A2-45F6-B0D9-5D0E5AFA1D88"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"), Description = "Awesome Desc", CurrentPriceId = 10001 },
            new FurnitureUnit(code: "EXFA", width: 2, height: 3, depth: 4) { Id = new Guid("FFFC8FEF-6FF2-4D85-B63B-90B156B49055"), CategoryId = 10000, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"), Description = "Cool Desc", CurrentPriceId = 10001 },
        };
    }
}
