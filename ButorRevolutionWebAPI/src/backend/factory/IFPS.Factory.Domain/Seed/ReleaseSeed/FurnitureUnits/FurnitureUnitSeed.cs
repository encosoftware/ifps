using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class FurnitureUnitSeed : IEntitySeed<FurnitureUnit>
    {
        public FurnitureUnit[] Entities => new[]
        {
            new FurnitureUnit("EXSA", 1,1,1) { Id = new Guid("cbcc1b8d-2bf4-4ec6-a411-aa013ed19833"), CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), BaseFurnitureUnitId = null, Description = "Miracle Desc", CurrentPriceId = 1 },
            new FurnitureUnit("EXSE", 1,2,3) { Id = new Guid("42dfb2f8-c914-48c1-a0b8-cf40806b24db"), CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), BaseFurnitureUnitId = new Guid("cbcc1b8d-2bf4-4ec6-a411-aa013ed19833"), Description = "Brilliant Desc", CurrentPriceId = 2 },
            new FurnitureUnit("FU-503", 10,20,30) { Id = new Guid("5799dc7e-f9cb-47a3-8d1c-3cfbc79af794"), CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), BaseFurnitureUnitId = new Guid("cbcc1b8d-2bf4-4ec6-a411-aa013ed19833"), Description = "Sparkling unit", CurrentPriceId = 2 },
     
            new FurnitureUnit("X5L", 1000,200,300) { Id = new Guid("51b66111-d87d-457e-ac05-f451e942165f"), CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), BaseFurnitureUnitId = null, Description = "xxl file test unit", CurrentPriceId = 2 },
            new FurnitureUnit("Y5L", 600,200,300) { Id = new Guid("8c757afa-2bfa-43a6-94c1-918c19675e64"), CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), BaseFurnitureUnitId = null, Description = "xxl file test unit 2", CurrentPriceId = 2 }
        };

        //public FurnitureUnit[] Entities => new FurnitureUnit[] { };
    }
}
