using System;

namespace IFPS.Sales.Domain.Enums
{
    [Flags]
    public enum NotificationTypeFlag
    {
        None = 0,
        SMS = 1,
        PushNotification = 2,
        Email = 4
    }
}
