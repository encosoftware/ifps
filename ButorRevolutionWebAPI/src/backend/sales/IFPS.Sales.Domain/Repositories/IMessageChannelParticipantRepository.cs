using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IMessageChannelParticipantRepository : IRepository<MessageChannelParticipant>
    {
        Task<List<int>> GetMessageChannelIdByuserIdAsync(int userId);
        Task<List<MessageChannelParticipant>> GetMessageChannelParticipantsAsync(int userId);
    }
}
