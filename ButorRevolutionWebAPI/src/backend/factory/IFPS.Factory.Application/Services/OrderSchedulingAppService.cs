using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class OrderSchedulingAppService : ApplicationService, IOrderSchedulingAppService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPlanRepository planRepository;
        private readonly IProductionProcessRepository productionProcessRepository;
        private readonly IOrderAppService orderAppService;
        private readonly IStockedMaterialRepository stockedMaterialRepository;

        public OrderSchedulingAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrderRepository orderRepository,
            IPlanRepository planRepository,
            IProductionProcessRepository productionProcessRepository,
            IOrderAppService orderAppService,
            IStockedMaterialRepository stockedMaterialRepository
            ) : base(aggregate)
        {
            this.orderRepository = orderRepository;
            this.planRepository = planRepository;
            this.productionProcessRepository = productionProcessRepository;
            this.orderAppService = orderAppService;
            this.stockedMaterialRepository = stockedMaterialRepository;
        }

        public async Task<ProductionStatusDetailsDto> GetProductionStatusByOrderIdAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderByWithConcretes(orderId);

            var planIds = new List<int>();
            foreach (var cfu in order.ConcreteFurnitureUnits)
            {
                foreach (var cfc in cfu.ConcreteFurnitureComponents)
                {
                    var plans = await planRepository.GetAllListAsync(ent => ent.ConcreteFurnitureComponentId == cfc.Id);
                    plans.ForEach(ent => planIds.Add(ent.Id));
                }
            }

            var processes = await productionProcessRepository.GetAllListIncludingAsync(ent => ent.OrderId == orderId && planIds.Contains(ent.PlanId), ent => ent.Plan.WorkStation.WorkStationType);
            return new ProductionStatusDetailsDto(processes);
        }

        public async Task<PagedListDto<OrderSchedulingListDto>> OrderSchedulingListAsync(OrderSchedulingFilterDto filterDto)
        {
            Expression<Func<Order, bool>> filter = (Order c) => c.OrderName != null;

            if (!string.IsNullOrEmpty(filterDto.OrderName))
            {
                filter = filter.And(ent => ent.OrderName.ToLower().Contains(filterDto.OrderName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.WorkingNumber))
            {
                filter = filter.And(ent => ent.WorkingNumber.ToLower().Contains(filterDto.WorkingNumber.ToLower().Trim()));
            }
            if (filterDto.OrderStatusId.HasValue)
            {
                filter = filter.And(ent => ent.CurrentTicket.OrderStateId == filterDto.OrderStatusId.Value);
            }
            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.From);
            }
            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.Deadline <= filterDto.To);
            }
            if (filterDto.Completion != 0)
            {
                filter = filter.And(ent => ent.Completion == filterDto.Completion);
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Order>(OrderSchedulingFilterDto.GetOrderingMapping(), nameof(Order.Id));

            var allOrders = await orderRepository.GetPagedOrderSchedulingAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);

            var result = allOrders.ToPagedList(OrderSchedulingListDto.FromEntity);
            foreach (var data in result.Data)
            {
                data.IsEnough = await SetIsEnoughValue(data.OrderId);
            }

            return result;
        }

        private async Task<bool> SetIsEnoughValue(Guid orderId)
        {
            var requiredMaterialDbos = await orderAppService.CalculateRequiredAmount(orderId);
            var stockedMaterials = stockedMaterialRepository.GetStockedMaterialsByIds(requiredMaterialDbos.Keys.ToList()).Result;

            if (stockedMaterials.Any(ent => ent.StockedAmount < requiredMaterialDbos.Single(e => e.Key == ent.MaterialId).Value.RequiredAmount))
            {
                return false;
            }

            return true;
        }
    }
}
