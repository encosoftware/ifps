using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class BoardMaterialTestSeed : IEntitySeed<BoardMaterial>
    {
        public BoardMaterial[] Entities => new[]
        {
            new BoardMaterial("TEST MATCODE", 10) { Id = new Guid("c649fc22-247e-4e88-817d-e398c349257b"), HasFiberDirection = false, CategoryId = 10001, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000 },
            new BoardMaterial("TEST BOARDMATCODE", 10) { Id = new Guid("b71e3923-3441-46c7-a2ba-13da290ecd6d"), HasFiberDirection = false, CategoryId = 10001, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000 },
            new BoardMaterial("TEST Material Code", 10) { Id = new Guid("b2e0b4a3-8327-4836-fff5-deaec8b3c93c"), HasFiberDirection = false, CategoryId = 10001, ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), CurrentPriceId = 10000, SiUnitId = 10000 }
        };
    }
}
