using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DivisionSeed : IEntitySeed<Division>
    {
        public Division[] Entities => new[]
        {
            new Division(DivisionTypeEnum.Admin) {Id = 1},
            new Division(DivisionTypeEnum.Production) {Id = 2},
            new Division(DivisionTypeEnum.Financial) {Id = 3},
            new Division(DivisionTypeEnum.Supply) {Id = 4},
            new Division(DivisionTypeEnum.Warehouse) {Id = 5}
        };
    }
}
