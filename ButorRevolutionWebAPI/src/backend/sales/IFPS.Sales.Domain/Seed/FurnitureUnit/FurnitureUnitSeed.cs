using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class FurnitureUnitSeed : IEntitySeed<FurnitureUnit>
    {
        public FurnitureUnit[] Entities => new[]
        {
            new FurnitureUnit("D1V_seed", 1,1,1) {Id = new Guid("791C05FC-5008-4785-A734-B9225BE8D3B3"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"),  Description ="Description for Furniture Unit 001", CurrentPriceId = 1, FurnitureUnitTypeId = 2},
            new FurnitureUnit("D2L_seed", 1,2,3) {Id = new Guid("a99749e2-d319-4be0-a4ff-d7b159c00f92"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), BaseFurnitureUnitId = new Guid("791c05fc-5008-4785-a734-b9225be8d3b3"), Description ="Description for Furniture Unit 002", CurrentPriceId = 2, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("A3V_seed", 8,12,9) { Id = new Guid("ebf87e4c-50b8-402c-b694-ad29183e0bb1"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 003", CurrentPriceId = 2, FurnitureUnitTypeId = 3 },
            new FurnitureUnit("C1VO_seed", 6,17,11) { Id = new Guid("4105025C-E947-4D82-8E72-216582EC6B94"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 004", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },

            // webshop
            new FurnitureUnit("E2VL_seed", 20,34,12) { Id = new Guid("BE472113-CE95-4C91-B5E4-6ACFEB54C639"), Trending = true, CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 005", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("A2VO_seed", 80,150,25) { Id = new Guid("B50D5D13-3885-4584-82C7-C267D848B893"), CategoryId = 2, ImageId = new Guid("cd8adf9e-8413-4b77-a0ba-9719ff9d0369"), Description ="Description for Furniture Unit 006", CurrentPriceId = 2, FurnitureUnitTypeId = 3 },
            new FurnitureUnit("B1V_seed", 42,180,30) { Id = new Guid("2598bb7c-e2fe-4999-9208-d57c4d0f643b"), CategoryId = 2, ImageId = new Guid("cd8adf9e-8413-4b77-a0ba-9719ff9d0369"), Description ="Description for Furniture Unit 007", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("D2V_seed", 20,80,37) { Id = new Guid("0537b101-5cdd-45d8-9def-accf331f19aa"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 008", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("D2V_water_seed", 20,40,37) { Id = new Guid("9AEC5347-3E49-4493-9CCA-825363858167"), Trending = true, CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 009", CurrentPriceId = 1, FurnitureUnitTypeId = 3 },
            new FurnitureUnit("A1L_seed", 60,200,60) { Id = new Guid("590f4957-136a-4ae3-a11c-88999e867b43"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 010", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("S1V_seed", 60,200,60) { Id = new Guid("44A6A5AC-2C90-49E2-8842-03132DB64231"), CategoryId = 2, Trending = true, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 011", CurrentPriceId = 1, FurnitureUnitTypeId = 1 },
            new FurnitureUnit("S2VO_seed", 60,200,60) { Id = new Guid("45e80e7b-1e2e-416a-b9c4-a2d97ffafe03"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 012", CurrentPriceId = 1, FurnitureUnitTypeId = 1 },
            new FurnitureUnit("V1V_seed", 60,200,60) { Id = new Guid("AA552298-6D89-4FB3-8DCA-5D67B768E64C"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 013", CurrentPriceId = 1, FurnitureUnitTypeId = 1 },

            new FurnitureUnit("K1VP1V_seed", 20,80,37) { Id = new Guid("7B949E08-6F04-48AA-AB26-F9685842459A"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 014", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("A1V2P_seed", 20,80,37) { Id = new Guid("45624A3C-1344-41C5-BCF4-20D4C218E1A1"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 015", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("D2L_oven_seed", 20,80,37) { Id = new Guid("061F03F7-B177-4F2A-90BA-AE7BA1B7B0E3"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 016", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("A2V_oven_seed", 20,80,37) { Id = new Guid("6E163C5A-CBB9-4416-9E2C-CA52B5801A72"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 017", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("E2VL_water_seed", 20,80,37) { Id = new Guid("D0675634-28E1-424E-BD8D-F5E07DB70263"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 018", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("V2V_seed", 20,80,37) { Id = new Guid("0BE9C957-2C66-4745-BE43-3AEA2085A3BB"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 019", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("I2V_seed", 20,80,37) { Id = new Guid("EB301B12-75D6-4634-BF9C-0AE1BA1B6281"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 020", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("I3VO_seed", 20,80,37) { Id = new Guid("CED5D4AB-0343-4C8A-A0DC-0676D1EAC76C"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 021", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("K1VO_seed", 20,80,37) { Id = new Guid("5F34F4F5-5BDD-4FA7-BC9E-F13F253DD063"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 022", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("T2V_seed", 20,80,37) { Id = new Guid("6EACE25E-A36C-4FD7-8341-53BD040ED370"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 023", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("R1VL_water_seed", 20,80,37) { Id = new Guid("A968069D-7F53-426D-AAD5-23C085784741"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 024", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("R2VO_seed", 20,80,37) { Id = new Guid("13A530EF-0FD8-4285-9094-5055908F8029"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 025", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
            new FurnitureUnit("A5V_seed", 20,80,37) { Id = new Guid("B48564AC-4C06-4128-9EF7-F827ABAA3E7B"), CategoryId = 2, ImageId = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), Description ="Description for Furniture Unit 026", CurrentPriceId = 1, FurnitureUnitTypeId = 2 },
        };
        //public FurnitureUnit[] Entities => new FurnitureUnit[] { };
    }
}
