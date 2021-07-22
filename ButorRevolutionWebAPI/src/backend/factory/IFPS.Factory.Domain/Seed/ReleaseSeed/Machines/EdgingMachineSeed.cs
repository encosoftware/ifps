using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class EdgingMachineSeed : IEntitySeed<EdgingMachine>
    {
        public EdgingMachine[] Entities => new[]
        {
            new EdgingMachine("Edging Machine 1", brandId: 5) { Id = 12 },
            new EdgingMachine("Edging Machine 2", brandId: 5) { Id = 13 },
            new EdgingMachine("Edging machine 3", brandId: 5)
            {
                Id = 17,
                SerialNumber = "1",
                SoftwareVersion = "1",
                YearOfManufacture = 2010,
                Code = "1"
            }
        };
    }
}
