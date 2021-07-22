using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class FoilMaterialSeed : IEntitySeed<FoilMaterial>
    {
        public FoilMaterial[] Entities => new[]
        {
            new FoilMaterial("FL101021WQ (seed)",10) { Id = new Guid("b2599856-ef0c-491e-9c4f-d0244e90163c"),  Description = "Festett fehér élzáró fólia", Thickness = 2, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 1 },
            new FoilMaterial("FL0055BLC (seed)", 10) { Id = new Guid("794dbe07-22cd-42fb-8fce-de7ea9d45928"), Description = "Festett sötétszürke élzáró fólia", Thickness = 4, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 1  },
            new FoilMaterial("FL002345 (seed)", 10) { Id = new Guid("15b3184b-a389-4daf-b562-53d9ea4ad3bf"), Description = "Fémes fekete élzáró fólia", Thickness = 5, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 1  },
            new FoilMaterial("FL009995 (seed)", 10) { Id = new Guid("4b6fb38f-c242-4693-8614-8b684ce8de45"), Description = "Prémium fehért élzáró fólia", Thickness = 2, ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 1  }
        };

        //public FoilMaterial[] Entities => new FoilMaterial[] { };
    }
}