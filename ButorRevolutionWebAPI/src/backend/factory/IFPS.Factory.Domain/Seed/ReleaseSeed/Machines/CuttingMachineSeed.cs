using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CuttingMachineSeed : IEntitySeed<CuttingMachine>
    {
        public CuttingMachine[] Entities => new[]
        {
            new CuttingMachine("Cutting Machine 1", brandId: 5) { Id = 5 },
            new CuttingMachine("Cutting Machine 2", brandId: 5) { Id = 6 },
            new CuttingMachine("Cutting Machine 3", brandId: 5)
            {
                Id = 7,
                SerialNumber = "1",
                SoftwareVersion = "1",
                YearOfManufacture = 2010,
                Code = "1"
            }
        };
    }
}
