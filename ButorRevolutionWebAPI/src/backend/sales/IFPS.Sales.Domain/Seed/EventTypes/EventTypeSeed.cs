using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.EventTypes
{
    public class EventTypeSeed : IEntitySeed<EventType>
    {
        public EventType[] Entities => new[]
        {
            new EventType(EventTypeEnum.NewAppointment){ Id = 1 },
            new EventType(EventTypeEnum.AppointmentReminder){ Id = 2 },
            new EventType(EventTypeEnum.ChangedOrderState){ Id = 3 },
            new EventType(EventTypeEnum.NewFilesUploaded){ Id = 4 },
            new EventType(EventTypeEnum.NewMessages){ Id = 5 },
            new EventType(EventTypeEnum.OrderEvaluation){ Id = 6 }
        };
    }
}
