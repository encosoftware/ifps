using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Dbos.Trends;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IOrdersTrendRepository : IRepository<Order, Guid>
    {
        Task<List<OrderedFurnitureUnitsTrendsDbo>> GetOrdersGroupByFurnitureUnitsAggregated(int Take, DateTime? from, DateTime? to);
        Task<List<OrderedBoardMaterialsTrendDbo>> GetOrdersGroupByBoardMaterialsAggregated(FurnitureComponentTypeEnum FurnitureComponentType, int Take, DateTime? IntervalFrom, DateTime? IntervalTo);
    }
}
