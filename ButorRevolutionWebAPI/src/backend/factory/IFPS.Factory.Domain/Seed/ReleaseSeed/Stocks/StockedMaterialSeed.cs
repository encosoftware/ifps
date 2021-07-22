using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class StockedMaterialSeed : IEntitySeed<StockedMaterial>
    {
        public StockedMaterial[] Entities => new[]
        {
            new StockedMaterial(Guid.Parse("34132DCE-AC95-47D1-AB24-BEF35AE8EA4F"), amount: 100, minAmount: 20, reqAmount: 50) { Id = 1 },
            new StockedMaterial(Guid.Parse("4A7B9B0A-2299-4BB2-95AD-4F1B0F23A47F"), amount: 200, minAmount: 40, reqAmount: 60) { Id = 2 },
            new StockedMaterial(Guid.Parse("794DBE07-22CD-42FB-8FCE-DE7EA9D45928"), amount: 50, minAmount: 10, reqAmount: 20) { Id = 3 },
            new StockedMaterial(Guid.Parse("27CA32C4-38D1-4919-9790-3D8A4C9C4E3D"), amount: 1000, minAmount: 200, reqAmount: 400) { Id = 4 },
            new StockedMaterial(Guid.Parse("0B02521A-1442-4EAE-A41A-B65623502B60"), amount: 0, minAmount: 20, reqAmount: 10) { Id = 10 },

            new StockedMaterial(Guid.Parse("FB39BF09-E5E3-49C4-8E11-50782D5A5CAD"), amount: 1, minAmount: 1, reqAmount: 1) { Id = 5 }, // board
            new StockedMaterial(Guid.Parse("B2E5B9B1-2B4C-40C5-A178-B6368CF9F364"), amount: 40, minAmount: 20, reqAmount: 10) { Id = 6 }, // tipli
            new StockedMaterial(Guid.Parse("15B3184B-A389-4DAF-B562-53D9EA4AD3BF"), amount: 20, minAmount: 3, reqAmount: 1) { Id = 7 }, // foil 1
            new StockedMaterial(Guid.Parse("4B6FB38F-C242-4693-8614-8B684CE8DE45"), amount: 3, minAmount: 3, reqAmount: 1) { Id = 8 }, // foil 2
            new StockedMaterial(Guid.Parse("8C344649-0562-498C-95B2-2A3A784BB770"), amount: 14, minAmount: 15, reqAmount: 5) { Id = 9 }, // vasalat
        };

        //public StockedMaterial[] Entities => new StockedMaterial[] { };
    }
}
