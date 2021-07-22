using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CFCProductionStateTestSeed : IEntitySeed<CFCProductionState>
    {
        public CFCProductionState[] Entities => new[]
        {
            new CFCProductionState(CFCProductionStateEnum.Started) { Id = 10000 },
            new CFCProductionState(CFCProductionStateEnum.Finished) { Id = 10001 }
        };
    }
}
