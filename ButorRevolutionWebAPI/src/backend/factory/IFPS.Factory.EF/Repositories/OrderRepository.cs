using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
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
    public class OrderRepository : EFCoreRepositoryBase<IFPSFactoryContext, Order, Guid>, IOrderRepository
    {
        public OrderRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Order, object>>> DefaultIncludes => new List<Expression<Func<Order, object>>>
        {

        };

        public async Task<IPagedList<Order>> GetPagedOrderSchedulingAsync
            (Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var orders = GetAll()
                .Include(ent => ent.CurrentTicket.OrderState.Translations)
                .Where(ent => ent.CurrentTicket.OrderState.State == OrderStateEnum.WaitingForScheduling || ent.CurrentTicket.OrderState.State == OrderStateEnum.UnderMaterialReservation ||
                        ent.CurrentTicket.OrderState.State == OrderStateEnum.UnderProduction || ent.CurrentTicket.OrderState.State == OrderStateEnum.ProductionComplete ||
                        ent.CurrentTicket.OrderState.State == OrderStateEnum.Scheduled);

            return await GetPagedListAsync(orders, predicate, orderBy, pageIndex, pageSize);
        }

        public Task<IPagedList<Order>> GetOrdersByCompany
            (int companyId,
            Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Where(ent => ent.CompanyId == companyId
                && ent.CurrentTicket.OrderState.State == Domain.Enums.OrderStateEnum.WaitingForFirstPayment
                || ent.CurrentTicket.OrderState.State == Domain.Enums.OrderStateEnum.WaitingForSecondPayment)
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket.OrderState.Translations)
                .Include(ent => ent.FirstPayment)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.SecondPayment)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency);

            return GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<Order> GetOrderWithTicketsAsync(Guid id)
        {
            var result = await GetAll()
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)

                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)

                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)

                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .SingleOrDefaultAsync(ent => ent.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var result = await GetAll()
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)

                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)

                .Include(ent => ent.CurrentTicket.OrderState.Translations)
                .Include(ent => ent.FirstPayment.Price.Currency)
                .Include(ent => ent.SecondPayment.Price.Currency)
                .SingleAsync(ent => ent.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<List<Order>> GetAllOrdersWithSomeIncludes()
        {
            return await GetAll()
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.FurnitureUnit)
                        .ThenInclude(cfu => cfu.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)

                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfu => cfu.FurnitureComponent)

                .Include(ent => ent.CurrentTicket.OrderState)
                .Include(ent => ent.ConcreteApplianceMaterials)
                    .ThenInclude(oam => oam.ApplianceMaterial)
                        .ThenInclude(appmat => appmat.SellPrice)
                                .ThenInclude(price => price.Currency)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdForCutting(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.FurnitureUnit)
                        .ThenInclude(fu => fu.Components)

                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.FurnitureUnit)
                        .ThenInclude(fu => fu.Accessories)

                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)

                .Include(ent => ent.CurrentTicket.OrderState)
                .SingleAsync(ent => ent.Id == id);
        }

        public async Task<Order> GetOrderByIdForMaterialReservation(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.FurnitureUnit)
                        .ThenInclude(fu => fu.Accessories)
                            .ThenInclude(acc => acc.Accessory)

                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)
                            .ThenInclude(fc => fc.BoardMaterial)

                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)
                            .ThenInclude(fc => fc.TopFoil)
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)
                            .ThenInclude(fc => fc.BottomFoil)
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)
                            .ThenInclude(fc => fc.LeftFoil)
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                        .ThenInclude(cfc => cfc.FurnitureComponent)
                            .ThenInclude(fc => fc.RightFoil)

                .Include(ent => ent.ConcreteApplianceMaterials)
                .SingleAsync(ent => ent.Id == orderId);
        }

        public async Task<Order> GetOrderByWithConcretes(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.ConcreteFurnitureUnits)
                    .ThenInclude(cfu => cfu.ConcreteFurnitureComponents)
                .SingleAsync(ent => ent.Id == orderId);
        }

        public async Task<int> GetOldestYear()
        {
            var minDate = await GetAll()
                .Where(ent => ent.FirstPayment.PaymentDate != null)
                .MinAsync(ent => ent.FirstPayment.PaymentDate);

            return minDate.Value.Year;
        }
    }
}
