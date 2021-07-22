using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class DivisionSeed : IEntitySeed<Division>
    {
        public Division[] Entities => new[]
        {
            new Division(DivisionTypeEnum.Admin) {Id = 1},
            new Division(DivisionTypeEnum.Sales) {Id = 2},
            new Division(DivisionTypeEnum.Partner) {Id = 3},
            new Division(DivisionTypeEnum.Customer) {Id = 4}
        };
    }
}
