using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class NotificationAppService : ApplicationService, INotificationAppService
    {
        private readonly IEventTypeRepository eventTypeRepository;

        public NotificationAppService(IApplicationServiceDependencyAggregate aggregate,
            IEventTypeRepository eventTypeRepository) : base(aggregate)
        {
            this.eventTypeRepository = eventTypeRepository;
        }

        public async Task<List<EventTypeDto>> GetNotificationEventTypes()
        {
            return (await eventTypeRepository.GetAllEventsWithTranslations()).Select(ent => new EventTypeDto(ent)).ToList();
        }
    }
}
