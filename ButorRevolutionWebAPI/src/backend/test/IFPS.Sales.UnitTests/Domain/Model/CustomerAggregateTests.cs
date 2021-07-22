using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class CustomerAggregateTests
    {
        [Fact]
        public void Check_customer_initializing()
        {
            var date = DateTime.Now;
            var customer = new Customer(1, date);

            Assert.Equal(date, customer.ValidFrom);
            Assert.Null(customer.ValidTo);
            Assert.False(customer.IsDeleted);

            Assert.NotNull(customer.NotificationModes);
            Assert.Empty(customer.NotificationModes);
            Assert.Equal(NotificationTypeFlag.None, customer.NotificationType);
        }

        [Fact]
        public void Close_customer()
        {
            var date = DateTime.Now;
            var customer = new Customer(1, date);

            Assert.Equal(date, customer.ValidFrom);
            Assert.Null(customer.ValidTo);
            Assert.False(customer.IsDeleted);

            customer.Close();

            Assert.NotNull(customer.ValidTo);
        }

        [Fact]
        public void Add_new_notification_mode_success()
        {
            var customer = new Customer(1, DateTime.Now);

            customer.AddNotificationModes(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3);
            Assert.All(customer.NotificationModes, nm => Assert.Equal(customer.Id, nm.CustomerId));
        }

        [Fact]
        public void Delete_notification_mode_success()
        {
            var customer = new Customer(1, DateTime.Now);

            customer.AddNotificationModes(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3);

            customer.RemoveNotificationModes(new List<int>() { 2 });
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1 && !nm.IsDeleted);
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 2 && !nm.IsDeleted);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3 && !nm.IsDeleted);

            customer.RemoveNotificationModes(new List<int>() { 1 });
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 1 && !nm.IsDeleted);
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 2 && !nm.IsDeleted);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3 && !nm.IsDeleted);

            customer.RemoveNotificationModes(new List<int>() { 3 });
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 1 && !nm.IsDeleted);
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 2 && !nm.IsDeleted);
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 3 && !nm.IsDeleted);
        }

        [Fact]
        public void Add_notification_modes_more_than_once_success()
        {
            var customer = new Customer(1, DateTime.Now);

            customer.AddNotificationModes(new List<int>() { 1, 2 });

            Assert.Equal(2, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);

            customer.AddNotificationModes(new List<int>() { 2, 3 });

            Assert.Equal(3, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3);
        }

        [Fact]
        public void Add_duplicated_notification_modes_success()
        {
            var customer = new Customer(1, DateTime.Now);

            customer.AddNotificationModes(new List<int>() { 1, 2, 2 });

            Assert.Equal(2, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);
        }

        [Fact]
        public void Add_delete_add_notification_mode_success()
        {
            var customer = new Customer(1, DateTime.Now);

            customer.AddNotificationModes(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3);

            customer.RemoveNotificationModes(new List<int>() { 2 });
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1 && !nm.IsDeleted);
            Assert.DoesNotContain(customer.NotificationModes, nm => nm.EventTypeId == 2 && !nm.IsDeleted);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3 && !nm.IsDeleted);

            customer.AddNotificationModes(new List<int>() { 2 });
            Assert.Equal(4, customer.NotificationModes.Count());
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 1 && !nm.IsDeleted);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 2 && !nm.IsDeleted);
            Assert.Contains(customer.NotificationModes, nm => nm.EventTypeId == 3 && !nm.IsDeleted);
        }
    }
}