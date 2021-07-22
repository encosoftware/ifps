using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class NotificationsDto
    {
        public List<string> NotificationTypeFlags { get; set; }
        public List<int> EventTypeIds { get; set; }

        public static NotificationsDto FromModel(Customer customer)
        {
            return new NotificationsDto
            {
                NotificationTypeFlags = customer.NotificationType.ToString().Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                EventTypeIds = customer.NotificationModes.Select(nm => nm.EventTypeId).ToList(),
            };
        }
    }
}
