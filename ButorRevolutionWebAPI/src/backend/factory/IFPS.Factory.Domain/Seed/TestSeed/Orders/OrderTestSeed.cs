using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderTestSeed : IEntitySeed<Order>
    {
        public Order[] Entities => new[]
        {
            new Order("Test Super Order") { Id = new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"), WorkingNumber = "C43DF58", Deadline = new DateTime(2019, 10, 1), CurrentTicketId = 10000, CompanyId = 10000 },

            // Order Scheduling
            new Order("Test Bestellung-1203", new DateTime(2019, 8, 25)) { Id = new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"), CurrentTicketId = 10000, WorkingNumber = "MZ/X-503", CompanyId = 10000, Completion = 0, EstimatedProcessTime = 60000 },
            new Order("Test Bestellung-9357", new DateTime(2019, 8, 25)) { Id = new Guid("5bab1970-41de-428f-876f-ac220bd0e7b1"), CurrentTicketId = 10000, WorkingNumber = "MZ/X-880", CompanyId = 10000, Completion = 42, EstimatedProcessTime = 42000 },
            new Order("Test Bestellung-3990", new DateTime(2019, 8, 25)) { Id = new Guid("b8dd50e9-155d-449a-a45f-d027a2d43eba"), CurrentTicketId = 10000, WorkingNumber = "MZ/X-007", CompanyId = 10000, Completion = 100, EstimatedProcessTime = 120000 },

            // ProductionProcess/Plan
            new Order("ProductionProcess Test Order")
            {
                Id = new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"),
                WorkingNumber = "2020-MZ/X-77",
                CurrentTicketId = 10000,
                CompanyId = 10000
            }
        };
    }
}