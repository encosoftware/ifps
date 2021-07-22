using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class TicketSeed : IEntitySeed<Ticket>
    {
        public Ticket[] Entities => new[]
        {
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 1},
            new Ticket(orderStateId: 10, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 2},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 3},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 4},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 5},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 6},
            new Ticket(orderStateId: 5, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 7},

            //WaitingForSecondPayment
            new Ticket(orderStateId: 10, assignedTo: 1, deadline: Clock.Now.AddMonths(1), validTo: Clock.Now.AddMonths(1)) {Id = 8},

            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 9},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 10},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 11},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 12},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 13},
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1)) {Id = 14},
            
            //WaitingForFirstPayment
            new Ticket(orderStateId: 2, assignedTo: 1, deadline: Clock.Now.AddMonths(1), orderId: new Guid("3B290920-BDA8-41DA-BF7E-B3F8770CD5B5"), validTo: Clock.Now.AddMonths(1)) {Id = 15 },

            //FirstPaymentConfirmed
            new Ticket(orderStateId: 3, assignedTo: 1, deadline: Clock.Now.AddMonths(1), orderId: new Guid("3B290920-BDA8-41DA-BF7E-B3F8770CD5B5"), validTo: Clock.Now.AddMonths(1)) {Id = 16 },

            //Second payment confirmed
            new Ticket(orderStateId: 10, assignedTo: 1, deadline: Clock.Now.AddMonths(1), orderId: new Guid("3B290920-BDA8-41DA-BF7E-B3F8770CD5B5"), validTo: Clock.Now.AddMonths(1)) {Id = 18 },
        };

        //public Ticket[] Entities => new Ticket[] { };
    }
}
