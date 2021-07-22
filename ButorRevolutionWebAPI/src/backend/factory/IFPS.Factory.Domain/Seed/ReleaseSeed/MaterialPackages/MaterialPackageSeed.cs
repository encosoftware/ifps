using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class MaterialPackageSeed : IEntitySeed<MaterialPackage>
    {
        public MaterialPackage[] Entities => new[]
        {
            new MaterialPackage(new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), 3) { Id = 1, PackageCode = "BL100200FG 1brd (seed)", PackageDescription = "BL100200FG tölgy tábla", Size = 1 },
            new MaterialPackage(new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), 2) { Id = 2, PackageCode = "LX-135DX 30pcs (seed)", PackageDescription = "Kilincs 30-as csomag", Size = 30 },
            new MaterialPackage(new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"), 2) { Id = 3, PackageCode = "FL12034KP 50m (seed)", PackageDescription = "FL12034KP 50m tekercs festett szürke fólia", Size = 1 },
            new MaterialPackage(new Guid("0b02521a-1442-4eae-a41a-b65623502b60"), 3) { Id = 4, PackageCode = "LX-005SX 15pcs (seed)", PackageDescription = "12 cm-es sarokvas 15-ös csomag", Size = 15 },

            // accessory
            new MaterialPackage(new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), 3) { Id = 5, PackageCode = "LX-001 20pcs (seed)", PackageDescription = "Fatipli 20-as csomag", Size = 20 },
            new MaterialPackage(new Guid("b2e5b9b1-2b4c-40c5-a178-b6368cf9f364"), 3) { Id = 6, PackageCode = "LX-001 80pcs (seed)", PackageDescription = "Fatipli 80-as csomag", Size = 80 },
            new MaterialPackage(new Guid("8c344649-0562-498c-95b2-2a3a784bb770"), 3) { Id = 7, PackageCode = "LX-010 5pcs (seed)", PackageDescription = "Vasalat 5-ös csomag", Size = 5 },
            // decor board
            new MaterialPackage(new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"), 3) { Id = 8, PackageCode = "BL00712098 1brd (seed)", PackageDescription = "BL00712098 fenyő tábla", Size = 1 },
            // foil
            new MaterialPackage(new Guid("4b6fb38f-c242-4693-8614-8b684ce8de45"), 2) { Id = 9, PackageCode = "FL009995 50m (seed)", PackageDescription = "FL009995 50m tekercs laminált fehér élzáró fólia", Size = 50 },
            new MaterialPackage(new Guid("4b6fb38f-c242-4693-8614-8b684ce8de45"), 2) { Id = 10, PackageCode = "FL009995 100m (seed)", PackageDescription = "FL009995 100m tekercs laminált fehér élzáró fólia", Size = 100 },
            new MaterialPackage(new Guid("15b3184b-a389-4daf-b562-53d9ea4ad3bf"), 3) { Id = 11, PackageCode = "FL002345 70m (seed)", PackageDescription = "FL002345 70m fémes szürke élzáró fólia", Size = 70 },

        };

        //public MaterialPackage[] Entities => new MaterialPackage[] { };

    }
}
