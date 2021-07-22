using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class EventTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public EventTypeDto(EventType eventType)
        {
            Id = eventType.Id;
            Name = eventType.CurrentTranslation.Name;
            Description = eventType.CurrentTranslation.Name;
        }
    }
}
