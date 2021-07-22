using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<MaterialPackage> GetMachineStoredMaterialPackage(int storageCellId, Guid materialId);
    }
}
