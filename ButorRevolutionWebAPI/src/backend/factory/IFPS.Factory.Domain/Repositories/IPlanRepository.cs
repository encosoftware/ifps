using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Task<List<Plan>> GetPlansByConcreteId(List<int> concreteFurnitureUnitIds, List<int> concreteFurnitureComponentIds);
    }
}
