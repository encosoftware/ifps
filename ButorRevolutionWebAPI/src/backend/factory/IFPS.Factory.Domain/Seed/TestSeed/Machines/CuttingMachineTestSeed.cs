using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CuttingMachineTestSeed : IEntitySeed<CuttingMachine>
    {
        public CuttingMachine[] Entities => new[]
        {
            new CuttingMachine("Test Cutting machine", brandId: 10000) { Id = 10006 },
            new CuttingMachine("Test Cutting machine with unicorn", brandId: 10000) { Id = 10007 },
            new CuttingMachine("Test Cutting machine with rainbow", brandId: 10000) { Id = 10008 },
            new CuttingMachine("Test Cutting machine with frogs", brandId: 10000)
            {
                Id = 10009,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            },

            new CuttingMachine("Test Cutting machine with dragons", brandId: 10000)
            {
                Id = 10011,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            }
        };
    }
}
