using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class OrderStateTypeAppService : ApplicationService, IOrderStateTypeAppService
    {
        private readonly IOrderStateRepository orderStateTypeRepository;

        public OrderStateTypeAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IOrderStateRepository orderStateTypeRepository) : base(aggregate)
        {
            this.orderStateTypeRepository = orderStateTypeRepository;
        }

        public async Task<List<OrderStateTypeDropdownListDto>> GetFinanceOrderStatesAsync()
        {
            var financeOrderStatuses = await orderStateTypeRepository.GetStatusesForFinanceAsync();
            return financeOrderStatuses.Select(ent => new OrderStateTypeDropdownListDto(ent.Id, ent.CurrentTranslation.Name)).ToList();
        }

        public async Task<List<OrderStateTypeDropdownListDto>> GetOrderSchedulingOrderStatesAsync()
        {
            var statuses = await orderStateTypeRepository.GetStatusesForOrderSchedulingAsync();
            return statuses.Select(ent => new OrderStateTypeDropdownListDto(ent.Id, ent.CurrentTranslation.Name)).ToList();
        }

        public async Task<List<OrderStateTypeDropdownListDto>> GetOrderStateTypeDropdownListAsync()
        {
            var statuses = await orderStateTypeRepository.GetAllListAsync();

            var statusList = new List<OrderStateTypeDropdownListDto>();

            foreach (var status in statuses)
            {
                var statusType = new OrderStateTypeDropdownListDto(status.Id, status.CurrentTranslation.Name);
                statusList.Add(statusType);
            }

            return statusList;
        }
    }
}
