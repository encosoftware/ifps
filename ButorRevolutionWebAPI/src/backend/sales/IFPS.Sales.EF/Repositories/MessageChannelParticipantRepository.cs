using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class MessageChannelParticipantRepository : EFCoreRepositoryBase<IFPSSalesContext, MessageChannelParticipant>, IMessageChannelParticipantRepository
    {
        public MessageChannelParticipantRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<MessageChannelParticipant, object>>> DefaultIncludes => new List<Expression<Func<MessageChannelParticipant, object>>>
        {
        };

        public async Task<List<MessageChannelParticipant>> GetMessageChannelParticipantsAsync(int userId)
        {
            return await GetAll()
                .Where(ent => ent.UserId == userId)
                .Include(ent => ent.ParticipantMessages)
                .ThenInclude(ent => ent.Message)
                .ToListAsync();
        }

        public async Task<List<int>> GetMessageChannelIdByuserIdAsync(int userId)
        {
            return await context.MessageChannelParticipants
                   .Where(ent => ent.UserId == userId)
                   .Select(ent => ent.MessageChannelId)
                   .ToListAsync();
        }
    }
}
