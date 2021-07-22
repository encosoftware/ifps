using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderedMaterialPackageTestSeed : IEntitySeed<OrderedMaterialPackage>
    {
        public OrderedMaterialPackage[] Entities => new[]
        {
            new OrderedMaterialPackage() { Id = 10000, CargoId = 10002, OrderedAmount = 20, MaterialPackageId = 10000 },
            new OrderedMaterialPackage() { Id = 10001, CargoId = 10002, OrderedAmount = 15, MaterialPackageId = 10001 },
            new OrderedMaterialPackage() { Id = 10002, CargoId = 10001, OrderedAmount = 15, MaterialPackageId = 10001 },
            new OrderedMaterialPackage() { Id = 10003, CargoId = 10003, OrderedAmount = 15, MaterialPackageId = 10001 },
            new OrderedMaterialPackage() { Id = 10004, CargoId = 10004, OrderedAmount = 24, MaterialPackageId = 10000 },
            new OrderedMaterialPackage() { Id = 10005, CargoId = 10004, OrderedAmount = 39, MaterialPackageId = 10001 }
        };
    }
}
