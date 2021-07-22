using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CncMachineSeed : IEntitySeed<CncMachine>
    {
        public CncMachine[] Entities => new[]
        {
            new CncMachine("CNC machine 1", brandId: 5)
            { 
                Id = 3,
                SerialNumber = "003",
                SoftwareVersion = "1.5",
                YearOfManufacture = 2012,
                ReturnHeight = 1,
                Code = "S002ER45",
                SharedFolderPath = "X:\\ButorRevolution\\Development\\Documents\\MockCncMachines\\04"
            },
            new CncMachine("CNC machine 2", brandId: 5)
            {
                Id = 4,
                SerialNumber = "004",
                SoftwareVersion = "3.7.8",
                YearOfManufacture = 2015,
                ReturnHeight = 1,
                Code = "S002KI98",
                SharedFolderPath = "X:\\ButorRevolution\\Development\\Documents\\MockCncMachines\\03"
            },
            new CncMachine("CNC machine 3", brandId: 5)
            {
                Id = 8,
                SerialNumber = "001",
                SoftwareVersion = "3.14",
                ReturnHeight = 1,
                YearOfManufacture = 2018,
                Code = "D120HN67",
                SharedFolderPath = "X:\\ButorRevolution\\Development\\Documents\\MockCncMachines\\01"
            },
            new CncMachine("CNC machine 4", brandId: 5)
            {
                Id = 9,
                SerialNumber = "002",
                SoftwareVersion = "5.78-2",
                ReturnHeight = 1,
                YearOfManufacture = 2017,
                Code = "DR003T45",
                SharedFolderPath = "X:\\ButorRevolution\\Development\\Documents\\MockCncMachines\\02"
            }
        };
    }
}
