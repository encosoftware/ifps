using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CncMachineTestSeed : IEntitySeed<CncMachine>
    {
        public CncMachine[] Entities => new[]
        {
            new CncMachine("Test CNC machine", brandId: 10000)
            {
                Id = 10002,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            },

            new CncMachine("Test CNC machine with fairy", brandId: 10000)
            {
                Id = 10003,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            },

            new CncMachine("Test CNC machine with dwarf", brandId: 10000) { Id = 10004 },
            new CncMachine("Test CNC machine with elf", brandId: 10000) { Id = 10005 },
            new CncMachine("Test CNC controller machine", brandId: 10000)
            {
                Id = 10010,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            },
            new CncMachine("Test CNC controller machine", brandId: 10000)
            {
                Id = 10012,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            },
        };
    }
}
