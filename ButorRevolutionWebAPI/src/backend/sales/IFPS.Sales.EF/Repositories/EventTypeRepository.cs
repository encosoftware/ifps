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
    public class EventTypeRepository : EFCoreRepositoryBase<IFPSSalesContext, EventType>, IEventTypeRepository
    {
        public EventTypeRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<EventType, object>>> DefaultIncludes => new List<Expression<Func<EventType, object>>>
        {
            ent => ent.Translations,
        };

        public Task<List<EventType>> GetAllEventsWithTranslations()
        {
            return GetAll()              
                .ToListAsync();
        }

        public Task<List<EventType>> GetEventTypesByIdsAsync(ICollection<int> ids)
        {
            return GetAll()
                .Where(et => ids.Any(id => et.Id == id))
                .ToListAsync();
        }
    }
}
