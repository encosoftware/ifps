using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using LinqKit;

namespace IFPS.Sales.Application.Services
{
    public class StatisticsAppService : ApplicationService, IStatisticsAppService
    {
        private readonly IOrderRepository orderRepository;

        public StatisticsAppService(IOrderRepository orderRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<SalesPersonStatisticsDto> GetSalesPersonsListAsync(SalesPersonFilterDto filterDto)
        {
            var statisticsDto = new SalesPersonStatisticsDto();
            Expression<Func<Order, bool>> filter = CreateFilter(filterDto);

            var orders = await orderRepository.GetAllListIncludingAsync(filter, ent => ent.SalesPerson.User.CurrentVersion, ent => ent.FirstPayment.Price.Currency, ent => ent.SecondPayment.Price.Currency);

            if (orders.Count > 0)
            {
                var groupedBySalesPersons = orders.GroupBy(ent => ent.SalesPerson.User.CurrentVersion.Name);

                foreach (var group in groupedBySalesPersons)
                {
                    var salesPersonListDto = new SalesPersonListDto() { Name = group.Key };
                    foreach (var order in group)
                    {
                        if (order.ContractDate.HasValue)
                        {
                            ++salesPersonListDto.NumberOfContracts;
                            salesPersonListDto.SetTotal(order.FirstPayment.Price, order.SecondPayment.Price);
                        }
                        ++salesPersonListDto.NumberOfOffers;
                    }

                    salesPersonListDto.Efficiency = salesPersonListDto.NumberOfContracts == 0
                        ? (decimal)0
                        : Convert.ToDecimal((100.0 / salesPersonListDto.NumberOfContracts) * salesPersonListDto.NumberOfOffers);

                    statisticsDto.SalesPersons.Add(salesPersonListDto);
                }

                statisticsDto.Summary.FromEntities(statisticsDto.SalesPersons);
            }

            return statisticsDto;
        }

        private static Expression<Func<Order, bool>> CreateFilter(SalesPersonFilterDto filterDto)
        {
            Expression<Func<Order, bool>> filter = (Order ent) => ent.ContractDate != null;

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                filter = filter.And(ent => ent.SalesPerson.User.CurrentVersion.Name.ToLower().Contains(filterDto.Name.ToLower().Trim()));
            }

            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.ContractDate >= filterDto.From.Value);
            }

            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.ContractDate <= filterDto.From.Value);
            }

            return filter;
        }
    }
}
