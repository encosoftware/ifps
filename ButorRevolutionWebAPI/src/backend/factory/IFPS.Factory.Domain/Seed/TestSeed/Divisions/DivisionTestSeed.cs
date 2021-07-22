using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DivisionTestSeed : IEntitySeed<Division>
    {
        public Division[] Entities => new[]
        {
            new Division(DivisionTypeEnum.Admin) {Id = 10000},
            new Division(DivisionTypeEnum.Production) {Id = 10001},
            new Division(DivisionTypeEnum.Financial) {Id = 10002},
            new Division(DivisionTypeEnum.Supply) {Id = 10003},
            new Division(DivisionTypeEnum.Warehouse) {Id = 10004}
        };
    }
}
