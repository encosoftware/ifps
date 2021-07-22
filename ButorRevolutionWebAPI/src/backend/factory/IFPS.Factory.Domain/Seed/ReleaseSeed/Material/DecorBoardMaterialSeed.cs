using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class DecorBoardMaterialSeed : IEntitySeed<DecorBoardMaterial>
    {
        public DecorBoardMaterial[] Entities => new[]
        {
            new DecorBoardMaterial("BL2345TR (seed)",10) { Id = new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), CategoryId = 1, Description= "Premium akrill kék bútorlap",  HasFiberDirection = false, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") },
            new DecorBoardMaterial("BLA00120 (seed)",8) { Id = new Guid("0b02521a-1442-4eae-a41a-b65623502b60"), CategoryId = 1, Description= "Akrill barna bútorlap",  HasFiberDirection = false, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") },
            new DecorBoardMaterial("BLO9980FG (seed)", 1) { Id = new Guid("62d6de6f-3e2f-4549-b219-c9b0eeaa5424"), CategoryId = 1, Description= "Csiszolt tölgy bútorlap", HasFiberDirection = false, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") },

            new DecorBoardMaterial("BL00712098 (seed)") { Id = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"), CategoryId = 1, Description= "Premium fenyő bútorlap", HasFiberDirection = false, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb") },
        };

        //public DecorBoardMaterial[] Entities => new DecorBoardMaterial[] { };
    }
}