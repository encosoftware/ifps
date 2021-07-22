using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Enums;
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
    public class OrderStateRepository : EFCoreRepositoryBase<IFPSSalesContext, OrderState>, IOrderStateRepository
    {
        public OrderStateRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<OrderState, object>>> DefaultIncludes => new List<Expression<Func<OrderState, object>>>
        {
            os=>os.Translations,
        };

        public async Task<List<OrderState>> GetOrderStatuses()
        {
            return await GetAll()
                .Where(ent => ent.State == OrderStateEnum.WaitingForOffer ||
                            ent.State == OrderStateEnum.WaitingForOfferFeedback ||
                            ent.State == OrderStateEnum.WaitingForContract ||
                            ent.State == OrderStateEnum.WaitingForContractFeedback ||
                            ent.State == OrderStateEnum.UnderProduction ||
                            ent.State == OrderStateEnum.WaitingForShipping ||
                            ent.State == OrderStateEnum.Delivered ||
                            ent.State == OrderStateEnum.Installed)
                .ToListAsync();
        }
    }
}
