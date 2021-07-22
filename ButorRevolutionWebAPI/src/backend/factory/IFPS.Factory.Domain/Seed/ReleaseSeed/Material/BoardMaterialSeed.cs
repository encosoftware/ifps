using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class BoardMaterialSeed : IEntitySeed<BoardMaterial>
    {
        // DEPRECATED CLASS DON'T USE IT

        //public BoardMaterial[] Entities => new[]
        //{
        //    new BoardMaterial("", 10) { Id = new Guid("aefc6217-2971-4b4c-bf4e-8d39dc4aad0e"), HasFiberDirection = false, CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") },
        //    new BoardMaterial("BLHAT5627 (seed)", 10) { Id = new Guid("3bebe52d-5eeb-4df3-9193-739c05a1fc06"), HasFiberDirection = false, CategoryId = 1, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") }
        //};
        public BoardMaterial[] Entities => new BoardMaterial[] { };
    }
}
