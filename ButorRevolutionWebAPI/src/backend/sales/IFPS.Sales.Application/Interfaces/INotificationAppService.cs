using IFPS.Sales.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface INotificationAppService
    {
        Task<List<EventTypeDto>> GetNotificationEventTypes();
    }
}
