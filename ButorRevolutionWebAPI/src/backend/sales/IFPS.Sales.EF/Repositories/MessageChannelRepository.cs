using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Repositories;
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
    public class MessageChannelRepository : EFCoreRepositoryBase<IFPSSalesContext, MessageChannel>, IMessageChannelRepository
    {
        public MessageChannelRepository(IFPSSalesContext context) : base(context)
        {
        }
        protected override List<Expression<Func<MessageChannel, object>>> DefaultIncludes => new List<Expression<Func<MessageChannel, object>>>
        {
        };

        public async Task<MessageChannel> GetMessageChannelAsync(int id)
        {
            var result = await GetAll()
                .Include(ent => ent.Participants)
                    .ThenInclude(ent => ent.ParticipantMessages)
                        .ThenInclude(ent => ent.Message)
                            .ThenInclude(ent => ent.Sender.User.CurrentVersion)
                .Include(ent => ent.Participants)
                    .ThenInclude(ent => ent.User.CurrentVersion)
                .SingleAsync(ent => ent.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(MessageChannel), id);
        }

        public /*async*/ Task<List<MessageChannel>> GetMessageChannelsByUserIdAndOrderIdAsync(Guid orderId, int userId)
        {
            throw new NotImplementedException();

            //var result = await GetAll()
            //    //.Include(ent => ent.Participants)
            //    //    .ThenInclude(ent => ent.ParticipantMessages)
            //    //        .ThenInclude(ent => ent.Message)
            //    //            .ThenInclude(ent => ent.Sender.Participant.CurrentVersion)
            //    //.Include(ent => ent.Participants)
            //    //    .ThenInclude(ent => ent.Participant.CurrentVersion)
            //    .Where(filter)
            //    .ToListAsync();

            //return result ?? throw new EntityNotFoundException(typeof(MessageChannel), userId);
        }

        public async Task<MessageChannelParticipant> GetMessageChannelParticipantByUserIdAsync(int messageChannelId, int userId)
        {
            return (await SingleIncludingAsync(ent => ent.Id == messageChannelId, ent => ent.Participants))
                .Participants.Single(p => p.UserId == userId);
        }
    }
}
