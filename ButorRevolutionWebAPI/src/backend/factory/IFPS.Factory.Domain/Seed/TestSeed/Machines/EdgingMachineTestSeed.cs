using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class EdgingMachineTestSeed : IEntitySeed<EdgingMachine>
    {
        public EdgingMachine[] Entities => new[]
       {
            new EdgingMachine("Test Edging machine", brandId: 10000) { Id = 10013 },
            new EdgingMachine("Test Edging machine with fairy", brandId: 10000) { Id = 10014 },
            new EdgingMachine("Test Edging machine with dwarf", brandId: 10000) { Id = 10015 },
            new EdgingMachine("Test Edging machine with elf", brandId: 10000)
            {
                Id = 10016,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            },
            new EdgingMachine("Test Edging controller machine", brandId: 10000)
            {
                Id = 10017,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            },
            new EdgingMachine("Test Edging controller machine", brandId: 10000)
            {
                Id = 10018,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            }
        };
    }
}
