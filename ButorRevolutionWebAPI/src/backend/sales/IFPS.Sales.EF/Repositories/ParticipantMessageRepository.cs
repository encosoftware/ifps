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
    public class ParticipantMessageRepository : EFCoreRepositoryBase<IFPSSalesContext, ParticipantMessage>, IParticipantMessageRepository
    {
        public ParticipantMessageRepository(IFPSSalesContext context) : base(context)
        {
        }
        protected override List<Expression<Func<ParticipantMessage, object>>> DefaultIncludes => new List<Expression<Func<ParticipantMessage, object>>>
        {
        };

        public async Task<List<ParticipantMessage>> GetParticipantMessagesByUser(int userId)
        {
            return await GetAll()
                .Where(ent => ent.Recipient.User.Id == userId)
                .Include(ent => ent.Recipient)
                    .ThenInclude(ent => ent.User)
                .Include(ent => ent.Message)
                    .ThenInclude(ent => ent.Sender)
                        .ThenInclude(ent => ent.User)
                            .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Message.MessageChannel)
                .ToListAsync();
        }

        public async Task<List<ParticipantMessage>> GetParticipantMessagesByOrderAndUser(Guid orderId, int userId)
        {
            return await GetAll()
                .Where(ent => ent.Recipient.User.Id == userId && ent.Message.MessageChannel.OrderId == orderId)
                .Include(ent => ent.Recipient)
                    .ThenInclude(ent => ent.User)
                .Include(ent => ent.Message)
                    .ThenInclude(ent => ent.Sender)
                        .ThenInclude(ent => ent.User)
                            .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Message.MessageChannel)
                .Include(ent => ent.Message.Sender.User.Image)
                .ToListAsync();
        }

        public async Task<int> GetCountedUnansweredMessagesByUser(int userId)
        {
            return await GetAll()
                .Where(ent => ent.Seen == false && ent.Recipient.UserId == userId)
                .CountAsync();
        }
    }
}
