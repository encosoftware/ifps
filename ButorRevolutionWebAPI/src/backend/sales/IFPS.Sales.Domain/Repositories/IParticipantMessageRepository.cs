using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IParticipantMessageRepository : IRepository<ParticipantMessage>
    {
        Task<List<ParticipantMessage>> GetParticipantMessagesByUser(int userId);
        Task<List<ParticipantMessage>> GetParticipantMessagesByOrderAndUser(Guid orderId, int userId);
        Task<int> GetCountedUnansweredMessagesByUser(int userId);
    }
}
