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
    public class MessageRepository : EFCoreRepositoryBase<IFPSSalesContext, Message>, IMessageRepository
    {
        public MessageRepository(IFPSSalesContext context) : base(context)
        {
        }
        protected override List<Expression<Func<Message, object>>> DefaultIncludes => new List<Expression<Func<Message, object>>>
        {            
        };

        public async Task<List<Message>> GetMessagesByUser(int id)
        {
            return await GetAll()
                .Include(ent => ent.Sender)
                .ThenInclude(ent => ent.User)
                .Include(d => d.MessageChannel)
                .Where(ent => ent.Sender.User.Id == id)
                .ToListAsync();
        }
    }
}
