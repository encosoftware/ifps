using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderPriceSeed : IEntitySeed<OrderPrice>
    {
        public OrderPrice[] Entities => new[]
        {
            new OrderPrice() { Id = 1, Deadline = new DateTime(2000, 3, 31), PaymentDate = new DateTime(2000, 2, 10) },
            new OrderPrice() { Id = 2, Deadline = new DateTime(1990, 10, 24), PaymentDate = new DateTime(1990, 10, 18) },
            new OrderPrice() { Id = 3, Deadline = new DateTime(2005, 3, 31), PaymentDate = new DateTime(2005, 2, 10) },
            new OrderPrice() { Id = 4, Deadline = new DateTime(2007, 3, 31), PaymentDate = new DateTime(2007, 2, 10) },
            new OrderPrice() { Id = 5, Deadline = new DateTime(2011, 3, 31), PaymentDate = new DateTime(2011, 2, 10) },
            new OrderPrice() { Id = 6, Deadline = new DateTime(1997, 3, 31), PaymentDate = new DateTime(1997, 2, 10) },
            new OrderPrice() { Id = 7, Deadline = new DateTime(1999, 1, 5), PaymentDate = new DateTime(1998, 12, 20) },
            new OrderPrice() { Id = 8, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 9, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 10, Deadline = new DateTime(2020, 3, 31) },
            new OrderPrice() { Id = 11, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 12, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 13, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 14, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 15, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 16, Deadline = new DateTime(2020, 3, 31) },
            new OrderPrice() { Id = 17, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 18, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 19, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 20, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 21, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 22, Deadline = new DateTime(2020, 3, 31), PaymentDate = Clock.Now.AddDays(14) },
            new OrderPrice() { Id = 23, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 24, Deadline = new DateTime(2020, 3, 31) },
            new OrderPrice() { Id = 25, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 26, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 27, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
            new OrderPrice() { Id = 28, Deadline = new DateTime(2020, 3, 31), PaymentDate = new DateTime(2020, 2, 10) },
        };

        //public OrderPrice[] Entities => new OrderPrice[] { };
    }
}
