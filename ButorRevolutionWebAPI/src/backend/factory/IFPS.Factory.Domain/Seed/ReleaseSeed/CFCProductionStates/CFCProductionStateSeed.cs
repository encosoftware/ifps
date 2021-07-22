using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CFCProductionStateSeed : IEntitySeed<CFCProductionState>
    {
        public CFCProductionState[] Entities => new[]
        {
            new CFCProductionState(CFCProductionStateEnum.Started) { Id = 1 },
            new CFCProductionState(CFCProductionStateEnum.Finished) { Id = 2 }
        };
    }
}
