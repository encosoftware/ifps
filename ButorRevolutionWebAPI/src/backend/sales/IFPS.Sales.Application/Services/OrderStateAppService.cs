using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class OrderStateAppService : ApplicationService, IOrderStateAppService
    {
        private readonly IOrderStateRepository orderStateRepository;
        public OrderStateAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrderStateRepository orderStateRepository) : base(aggregate)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task<List<OrderStateDto>> GetOrderStatesAsync()
        {
            return (await orderStateRepository.GetOrderStatuses()).Select(ent => new OrderStateDto(ent)).ToList();
        }
    }
}
