using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class AppointmentSeed : IEntitySeed<Appointment>
    {
        public Appointment[] Entities => new[]
        {
            new Appointment(null, 7, "Hétfő reggel megbeszélés (seed)")
            {
                Id = 1,
                OrderId = new Guid("5C75E657-4BB7-4791-A829-5E85C2EA7D12"),
                CustomerId = 1,
                MeetingRoomId = 1,
                CategoryId = 15,
                Notes = "Test notes 1"
            },

            new Appointment(null, 7, "Kedd reggel megbeszélés (seed)") { Id = 2, OrderId = new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), CustomerId = 1, CategoryId = 16 },
            new Appointment(null, 7, "Péntek reggel megbeszélés (seed)") {Id = 3, OrderId = new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), CustomerId = 1, CategoryId = 16 },
            new Appointment(null, 7, "Szerda reggel megbeszélés (seed)") {Id = 4, OrderId = new Guid("5C75E657-4BB7-4791-A829-5E85C2EA7D12"), CustomerId = 1, CategoryId = 16 }
        };
        //public Appointment[] Entities => new Appointment[] { };
    }
}
