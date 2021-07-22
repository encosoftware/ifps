using ENCO.DDD.Domain.Events;
using MediatR;

namespace IFPS.Factory.Domain.Events
{
    public interface IIFPSDomainEvent : INotification, IDomainEvent
    {
    }
}
