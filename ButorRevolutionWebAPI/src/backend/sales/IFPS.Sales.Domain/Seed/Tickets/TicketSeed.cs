using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class TicketSeed : IEntitySeed<Ticket>
    {
        public Ticket[] Entities => new[]
        {
            new Ticket(3,1,2, null, Clock.Now) {Id = 1},
            new Ticket(4,2,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), Clock.Now) {Id = 2},
            new Ticket(6,3,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), Clock.Now) {Id = 3},
            new Ticket(8,2,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), Clock.Now) {Id = 4},
            new Ticket(15,3,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), Clock.Now) {Id = 5},
            new Ticket(15,2,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"), Clock.Now) {Id = 6},
            new Ticket(3,1,2, null, Clock.Now) {Id = 7} ,
        };

        public Ticket[] OrderEntities => new[]
        {
            new Ticket(3,2,2 ,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),Clock.Now) {Id = 2},
            new Ticket(3,3,2 ,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),Clock.Now) {Id = 3},
            new Ticket(3,2,2 ,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),Clock.Now) {Id = 4},
            new Ticket(21,3,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),Clock.Now) {Id = 5},
            new Ticket(21,2,2,new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12"),Clock.Now) {Id = 6}
        };
        //public Ticket[] Entities => new Ticket[] { };
    }
}
