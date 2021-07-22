using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IStorageCellRepository : IRepository<StorageCell>
    {
        Task<List<Tuple<Guid, int>>> GetMaterialStock(List<Guid> matIds);
    }
}
