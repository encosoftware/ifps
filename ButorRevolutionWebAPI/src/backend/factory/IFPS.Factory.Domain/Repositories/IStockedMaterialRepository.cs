using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IStockedMaterialRepository : IRepository<StockedMaterial>
    {
        Task<List<StockedMaterial>> GetStockedMaterialsByIds(List<Guid> matIds);
    }
}
