using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class OrderStateRepository : EFCoreRepositoryBase<IFPSFactoryContext, OrderState>, IOrderStateRepository
    {
        public OrderStateRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<OrderState, object>>> DefaultIncludes => new List<Expression<Func<OrderState, object>>>
        {
            p => (OrderState)p.Translations
        };

        public async Task<List<OrderState>> GetStatusesForFinanceAsync()
        {
            return await GetAll()
                .Include(ent => ent.Translations)
                .Where(ent => ent.State == OrderStateEnum.WaitingForFirstPayment || ent.State == OrderStateEnum.WaitingForSecondPayment)
                .ToListAsync();
        }

        public async Task<List<OrderState>> GetStatusesForOrderSchedulingAsync()
        {
            return await GetAll()
                .Include(ent => ent.Translations)
                .Where(ent => ent.State == OrderStateEnum.ProductionComplete || ent.State == OrderStateEnum.UnderMaterialReservation ||
                        ent.State == OrderStateEnum.UnderProduction || ent.State == OrderStateEnum.WaitingForScheduling ||
                        ent.State == OrderStateEnum.Scheduled)
                .ToListAsync();
        }
    }
}
