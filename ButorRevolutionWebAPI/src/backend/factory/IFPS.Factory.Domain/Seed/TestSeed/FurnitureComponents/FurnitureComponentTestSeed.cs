using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class FurnitureComponentTestSeed : IEntitySeed<FurnitureComponent>
    {
        public FurnitureComponent[] Entities => new[]
        {
            new FurnitureComponent("Test Component from Earth", 5) { Id = new Guid("b3acef50-88cb-410f-a823-6d6b391611a5"), BoardMaterialId = new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), FurnitureUnitId = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47"), ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae") },
            new FurnitureComponent("Test Component from Outer Space", 42) { Id = new Guid("6dc5cac9-0339-429f-8302-41269b6192fe"), BoardMaterialId = new Guid("8b0936ec-0e49-4473-bb1d-2b8bb0564d34"), FurnitureUnitId = new Guid("5db3fb96-1619-4b05-afa2-9485c282db76"), ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae") },

            new FurnitureComponent("Reservation test 01", 1)
            { 
                Id = new Guid("4aa9bcaf-63e5-4cd6-aa6c-bf571fefb597"),
                Type = FurnitureComponentTypeEnum.Front,
                BoardMaterialId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"),
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"),
                FurnitureUnitId = new Guid("f4f37e65-379c-4569-a720-83e4f9ec0e90"),
                Width = 1000,
                Length = 600
            },
        };
    }
}
