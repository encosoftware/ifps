using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class TrendsAppService : ApplicationService, ITrendsAppService
    {
        private readonly IOrdersTrendRepository ordersTrendRepository;
        public TrendsAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrdersTrendRepository ordersTrendRepository) : base(aggregate)
        {
            this.ordersTrendRepository = ordersTrendRepository;
        }

        public async Task<TrendListDto<FurnitureUnitPreviewDto>> ListFurnitureUnitOrdersTrend(int Take, DateTime? IntervalFrom, DateTime? IntervalTo)
        {
            var res = await ordersTrendRepository.GetOrdersGroupByFurnitureUnitsAggregated(Take, IntervalFrom, IntervalTo);

            return new TrendListDto<FurnitureUnitPreviewDto>()
            {
                IntervalFrom = IntervalFrom,
                IntervalTo = IntervalTo,
                Take = Take,
                OrdersTotalCount = res.Sum(x => x.OrdersCount),
                Results = res.Select(x => new TrendListItemDto<FurnitureUnitPreviewDto>()
                {
                    OrdersCount = x.OrdersCount,
                    OrderedItemDto = FurnitureUnitPreviewDto.FromEntity(x.FurnitureUnit),
                }).ToList(),
            };
        }

        public async Task<TrendListDto<BoardMaterialPreviewDto>> ListBoardMaterialOrdersTrend(FurnitureComponentTypeEnum FurnitureComponentType, int Take, DateTime? IntervalFrom, DateTime? IntervalTo)
        {
            var res = await ordersTrendRepository.GetOrdersGroupByBoardMaterialsAggregated(FurnitureComponentType, Take, IntervalFrom, IntervalTo);

            return new TrendListDto<BoardMaterialPreviewDto>()
            {
                IntervalFrom = IntervalFrom,
                IntervalTo = IntervalTo,
                Take = Take,
                OrdersTotalCount = res.Sum(x => x.OrdersCount),
                Results = res.Select(x => new TrendListItemDto<BoardMaterialPreviewDto>()
                {
                    OrdersCount = x.OrdersCount,
                    OrderedItemDto = BoardMaterialPreviewDto.FromEntity(x.BoardMaterial),
                }).ToList(),
            };
        }
    }
}
