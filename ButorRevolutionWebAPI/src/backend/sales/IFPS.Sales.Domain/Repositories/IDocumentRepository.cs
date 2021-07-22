using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IDocumentRepository : IRepository<Document, Guid>
    {
        Task<int> GetCountOfSameTypeOfDocumentAsync(Guid orderId, int documentTypeId);
    }
}
