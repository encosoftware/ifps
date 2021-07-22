using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IMessageChannelRepository : IRepository<MessageChannel>
    {
        Task<MessageChannel> GetMessageChannelAsync(int id);
        Task<List<MessageChannel>> GetMessageChannelsByUserIdAndOrderIdAsync(Guid orderId, int userId);
        Task<MessageChannelParticipant> GetMessageChannelParticipantByUserIdAsync(int messageChannelId, int userId);
    }
}
