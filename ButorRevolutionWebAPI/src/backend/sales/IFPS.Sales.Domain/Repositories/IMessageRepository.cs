using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<Message>> GetMessagesByUser(int id);

    }
}
