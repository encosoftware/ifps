using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class DecorBoardMaterialSeed : IEntitySeed<DecorBoardMaterial>
    {
        public DecorBoardMaterial[] Entities => new[]
        {
            new DecorBoardMaterial("BL007001 (seed)",10) {Id = new Guid("7793fb49-f76c-49c4-ad17-85d8e7c92937"), CategoryId = 1, Description= "",  HasFiberDirection = false, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 1  },
            new DecorBoardMaterial("BL555GH (seed)", 10) {Id = new Guid("3b2edf78-39ef-440f-a780-9714685e51af"), CategoryId = 1, Description= "", HasFiberDirection = false, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 2  },
            new DecorBoardMaterial("BL200DL10 (seed)", 10) { Id = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"), CategoryId = 1, Description= "", HasFiberDirection = false, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 1 },
            new DecorBoardMaterial("BL990KK20 (seed)", 10) { Id = new Guid("b7ee9180-76a7-4854-9108-99e7d7295404"), CategoryId = 1, Description= "", HasFiberDirection = false, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c"), CurrentPriceId = 2 }
        };
        //public DecorBoardMaterial[] Entities => new DecorBoardMaterial[] { };
    }
}
