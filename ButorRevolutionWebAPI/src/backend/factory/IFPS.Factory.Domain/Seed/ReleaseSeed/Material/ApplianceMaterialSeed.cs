using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ApplianceMaterialSeed : IEntitySeed<ApplianceMaterial>
    {
        public ApplianceMaterial[] Entities => new[]
        {
            new ApplianceMaterial("AP180IA (seed)") { Id = new Guid("62d75a9f-702e-406b-bdc2-29152ca58f36"), BrandId = 1, CategoryId = 1, Description = "Elöltőltős mosógép MA180X 50cm", ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 1 },
            new ApplianceMaterial("AP192FF (seed)") { Id = new Guid("54153d0d-7d9e-4491-8e78-f505f6440e93"), BrandId = 1, CategoryId = 1, Description = "A+++ Mosogatógép, 120cm", ImageId = new Guid("78731aed-b8b6-4eef-baca-2855dcab25fb"), CurrentPriceId = 2 }
        };

        //public ApplianceMaterial[] Entities => new ApplianceMaterial[] { };
    }
}
