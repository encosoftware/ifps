using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderedMaterialPackageSeed : IEntitySeed<OrderedMaterialPackage>
    {
        public OrderedMaterialPackage[] Entities => new[]
        {
            new OrderedMaterialPackage() { Id = 1, CargoId = 1, MaterialPackageId = 1, OrderedAmount = 20, MissingAmount = 0, RefusedAmount = 0 },
            new OrderedMaterialPackage() { Id = 2, CargoId = 2, MaterialPackageId = 2, OrderedAmount = 14, MissingAmount = 0, RefusedAmount = 0 },
            new OrderedMaterialPackage() { Id = 3, CargoId = 4, MaterialPackageId = 3, OrderedAmount = 25, MissingAmount = 1, RefusedAmount = 2 },
            new OrderedMaterialPackage() { Id = 4, CargoId = 4, MaterialPackageId = 4, OrderedAmount = 18, MissingAmount = 5, RefusedAmount = 8 },
            new OrderedMaterialPackage() { Id = 5, CargoId = 5, MaterialPackageId = 3, OrderedAmount = 42, MissingAmount = 0, RefusedAmount = 0 },
            new OrderedMaterialPackage() { Id = 6, CargoId = 5, MaterialPackageId = 4, OrderedAmount = 70, MissingAmount = 0, RefusedAmount = 0 },
            new OrderedMaterialPackage() { Id = 7, CargoId = 3, MaterialPackageId = 4, OrderedAmount = 5, MissingAmount = 0, RefusedAmount = 0 },
            new OrderedMaterialPackage() { Id = 8, CargoId = 6, MaterialPackageId = 4, OrderedAmount = 10, MissingAmount = 0, RefusedAmount = 0 }
        };

        //public OrderedMaterialPackage[] Entities => new OrderedMaterialPackage[] { };
    }
}
