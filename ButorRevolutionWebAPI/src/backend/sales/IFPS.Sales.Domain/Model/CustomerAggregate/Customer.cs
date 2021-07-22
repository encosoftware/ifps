using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Customer : FullAuditedAggregateRoot
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        
        private List<CustomerNotificationMode> _notificationModes;
        public IEnumerable<CustomerNotificationMode> NotificationModes => _notificationModes.AsReadOnly();

        private List<CustomerFurnitureUnit> _recommendedProducts;
        public IEnumerable<CustomerFurnitureUnit> RecommendedProducts => _recommendedProducts.AsReadOnly();

        public NotificationTypeFlag NotificationType { get; set; }

        private Customer()
        {
            _notificationModes = new List<CustomerNotificationMode>();
            _recommendedProducts = new List<CustomerFurnitureUnit>();
        }

        public Customer(int userId, DateTime validFrom) : this()
        {
            UserId = userId;
            ValidFrom = validFrom;
        }

        public void Close(DateTime? validTo = null)
        {
            ValidTo = validTo ?? DateTime.Now;
        }

        public void AddNotificationModes(ICollection<int> eventTypesIds)
        {
            _notificationModes.AddRange(
                eventTypesIds.Where(eventTypeId => !_notificationModes.Any(nm => nm.EventTypeId == eventTypeId && !nm.IsDeleted))
                    .Select(eventTypeId => new CustomerNotificationMode(eventTypeId, this.Id))
            );
        }

        public void RemoveNotificationModes(ICollection<int> eventTypesIds)
        {
            foreach (var notification in _notificationModes.Where(nm => eventTypesIds.Any(EventTypeId => nm.EventTypeId == EventTypeId)))
            {
                notification.IsDeleted = true;
                notification.DeletionTime = Clock.Now;
            }
        }

        public void AddRecommendation(CustomerFurnitureUnit customerFurnitureUnit)
        {            
            Ensure.NotNull(customerFurnitureUnit);
            _recommendedProducts.Add(customerFurnitureUnit);
        }

        public void ClearRecommendations() => _recommendedProducts.Clear();        
    }
}
