using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class MaterialPackageTestSeed : IEntitySeed<MaterialPackage>
    {
        public MaterialPackage[] Entities => new[]
        {
            new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10000, PackageCode = "PAC-591", PackageDescription = "Package with stuff", Size = 1 },
            new MaterialPackage(new Guid("b2e0b4a3-8327-4836-fff5-deaec8b3c93c"), 10009) { Id = 10004, PackageCode = "PAC-591", PackageDescription = "Package with stuff", Size = 1 },

            new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10001, PackageCode = "COD-748", PackageDescription = "Stuff", Size = 1 },
            new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10002, PackageCode = "PDE-249", PackageDescription = "Furniture stuff", Size = 1 },
            new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10003, PackageCode = "GOD-777", PackageDescription = "Sofa stuff", Size = 1 }
        };
    }
}