using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IEventTypeRepository : IRepository<EventType>
    {
        Task<List<EventType>> GetEventTypesByIdsAsync(ICollection<int> ids);
        Task<List<EventType>> GetAllEventsWithTranslations();
    }
}
