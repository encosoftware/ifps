using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class MessageSeed : IEntitySeed<Message>
    {
        public Message[] Entities => new[]
        {
            new Message(1,1,"First message") {Id = 1},
            new Message(1,1,"Second message") {Id = 2},
            new Message(1,1,"Third message") {Id = 3},
            new Message(1,1,"Fourth message") {Id = 4},
            new Message(1,1,"Fifth message") {Id = 5},
            new Message(2,2,"First message") {Id = 6},
        };
        //public Message[] Entities => new Message[] { };
    }
}
