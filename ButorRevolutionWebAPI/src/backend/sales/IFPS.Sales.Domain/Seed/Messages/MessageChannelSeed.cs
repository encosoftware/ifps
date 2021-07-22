using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class MessageChannelSeed : IEntitySeed<MessageChannel>
    {
        public MessageChannel[] Entities => new[]
        {
            new MessageChannel(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12")) {Id = 1},
            new MessageChannel(new Guid("5c75e657-4bb7-4791-a829-5e85c2ea7d12")) {Id = 2}
        };
        //public MessageChannel[] Entities => new MessageChannel[] { };
    }
}
