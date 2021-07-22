using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class TicketTestSeed : IEntitySeed<Ticket>
    {
        public Ticket[] Entities => new[]
        {
            new Ticket(10007, 10000, new DateTime(2019, 7, 10)) { Id = 10000 }
        };
    }
}