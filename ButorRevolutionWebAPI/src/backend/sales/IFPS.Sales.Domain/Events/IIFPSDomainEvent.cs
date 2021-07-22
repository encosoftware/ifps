using ENCO.DDD.Domain.Events;
using MediatR;

namespace IFPS.Sales.Domain.Events
{
    public interface IIFPSDomainEvent : INotification, IDomainEvent
    {
    }
}
