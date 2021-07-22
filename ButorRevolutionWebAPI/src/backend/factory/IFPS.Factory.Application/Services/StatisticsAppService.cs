using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Extensions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class StatisticsAppService : ApplicationService, IStatisticsAppService
    {
        private readonly IStockRepository stockRepository;
        private readonly IGeneralExpenseRepository generalExpenseRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ICargoRepository cargoRepository;

        public StatisticsAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IStockRepository stockRepository,
            IGeneralExpenseRepository generalExpenseRepository,
            IOrderRepository orderRepository,
            ICargoRepository cargoRepository
            ) : base(aggregate)
        {
            this.stockRepository = stockRepository;
            this.generalExpenseRepository = generalExpenseRepository;
            this.orderRepository = orderRepository;
            this.cargoRepository = cargoRepository;
        }

        public async Task<StockStatisticsDetailsDto> GetStockStatisticsAsync(StockStatisticsFilterDto stockStatisticsFilterDto)
        {
            Expression<Func<Stock, bool>> filter = (Stock ent) => true;

            filter = filter.And(ent => ent.Package.MaterialId == stockStatisticsFilterDto.MaterialId);
            if (stockStatisticsFilterDto.DateFrom.HasValue && stockStatisticsFilterDto.DateTo.HasValue)
            {
                filter = filter.And(ent => (
                (ent.ValidTo > stockStatisticsFilterDto.DateFrom || ent.ValidTo == null) && ent.ValidFrom <= stockStatisticsFilterDto.DateFrom) ||
                (ent.ValidFrom < stockStatisticsFilterDto.DateTo && ent.ValidFrom >= stockStatisticsFilterDto.DateFrom)
                );
            }

            var stocks = await stockRepository.GetAllListAsync(filter, StockStatisticsDto.Projection);
            stocks = CalculateStockQuantity(stocks, stockStatisticsFilterDto.DateTo.Value);
            if (stocks.Count == 0)
            {
                return null;
            }
            else
            {
                return new StockStatisticsDetailsDto(stocks.Select(ent => ent.MaterialCode).First())
                {
                    Quantities = stocks.GroupBy(ent => GetWeekNumber(ent.DateFrom.Value), ent => ent.Quantity, (key, values) => new StockStatisticsQuantityDto(key, values.Sum())).ToList()
                };
            }
        }

        public List<StockStatisticsDto> CalculateStockQuantity(List<StockStatisticsDto> stocks, DateTime dateTo)
        {
            var newList = new List<StockStatisticsDto>();
            foreach (var stock in stocks)
            {
                stock.DateTo = stock.DateTo ?? Clock.Now;
                stock.DateTo = stock.DateTo > dateTo ? dateTo : stock.DateTo;
                stock.DateFrom = stock.DateFrom.Value.StartOfWeek(DayOfWeek.Monday);
                var difference = stock.DateTo.Value.DayOfYear - stock.DateFrom.Value.DayOfYear;
                for (int i = 0; i <= difference; i += 7)
                {
                    newList.Add(new StockStatisticsDto() { MaterialCode = stock.MaterialCode, DateFrom = stock.DateFrom, DateTo = stock.DateTo, Quantity = stock.Quantity });
                    stock.DateFrom = stock.DateFrom.Value.AddDays(7);
                }
            }
            return newList;
        }

        public async Task<List<FinanceStatisticsListDto>> GetFinanceStatisticsAsync(FinanceStatisticsFilterDto financeStatisticsFilterDto)
        {
            Expression<Func<Order, bool>> orderFilter = (Order ent) => ent.FirstPayment.PaymentDate != null;
            Expression<Func<Cargo, bool>> cargoFilter = (Cargo ent) => true;
            Expression<Func<GeneralExpense, bool>> expenseFilter = (GeneralExpense ent) => true;

            if (financeStatisticsFilterDto.DateFrom.HasValue && financeStatisticsFilterDto.DateTo.HasValue)
            {
                orderFilter = orderFilter.And(ent => (ent.FirstPayment.PaymentDate > financeStatisticsFilterDto.DateFrom && ent.FirstPayment.PaymentDate < financeStatisticsFilterDto.DateTo) || (ent.SecondPayment.PaymentDate > financeStatisticsFilterDto.DateFrom && ent.SecondPayment.PaymentDate < financeStatisticsFilterDto.DateTo));
                cargoFilter = cargoFilter.And(ent => ent.BookedOn > financeStatisticsFilterDto.DateFrom && ent.BookedOn < financeStatisticsFilterDto.DateTo);
                expenseFilter = expenseFilter.And(ent => ent.PaymentDate > financeStatisticsFilterDto.DateFrom && ent.PaymentDate < financeStatisticsFilterDto.DateTo);
            }
            var orders = await orderRepository.GetAllListIncludingAsync(orderFilter, ent => ent.FirstPayment.Price.Currency, ent => ent.SecondPayment.Price.Currency);
            var cargos = await cargoRepository.GetAllListIncludingAsync(cargoFilter, ent => ent.NetCost.Currency);
            var expenses = await generalExpenseRepository.GetAllListIncludingAsync(expenseFilter, ent => ent.Cost.Currency, ent => ent.GeneralExpenseRule);
            var currency = expenses.Select(ent => ent.Cost.Currency.Name).FirstOrDefault();
            var groupedOrders = orders.GroupBy(ent => ent.FirstPayment.PaymentDate.Value.Month, ent => ent.FirstPayment.Price, (key, values) => new FinanceStatisticsListDto() { Month = key, Income = values.Select(ent => ent.Value).Sum() }).ToList();
            var groupedCargos = cargos.GroupBy(ent => ent.BookedOn.Month, ent => ent.NetCost, (key, values) => new FinanceStatisticsListDto() { Month = key, GeneralExpenseCost = values.Select(ent => ent.Value).Sum() }).ToList();
            var groupedExpenses = expenses.GroupBy(ent => ent.PaymentDate.Month, ent => (ent.Cost, ent.GeneralExpenseRule), (key, values) => new FinanceStatisticsListDto()
            {
                Month = key,
                GeneralExpenseCost = values.Where(ent => ent.GeneralExpenseRule.ExpenseType == GeneralExpenseRuleEnum.Single).Select(ent => ent.Cost.Value).Sum(),
                RecurringCost = values.Where(ent => ent.GeneralExpenseRule.ExpenseType != GeneralExpenseRuleEnum.Single).Select(ent => ent.Cost.Value).Sum()
            }).ToList();

            var mergedList = groupedOrders.Union(groupedCargos).Union(groupedExpenses).GroupBy(ent => ent.Month, ent => ent, (key, values) => new FinanceStatisticsListDto()
            {
                Month = key,
                GeneralExpenseCost = values.Select(ent => ent.GeneralExpenseCost).Sum(),
                //RecurringCost = values.Select(ent => ent.RecurringCost).Sum(),
                RecurringCost = 0,
                Income = values.Select(ent => ent.Income).Sum(),
                Currency = currency
            }).ToList();
            return mergedList;
        }

        private static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public async Task<GetOldestYearDto> GetOldestYearAsync()
        {
            return new GetOldestYearDto(await orderRepository.GetOldestYear());
        }
    }
}
