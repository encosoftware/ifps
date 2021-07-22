using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Dbos.Trends;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class OrdersTrendRepository : EFCoreRepositoryBase<IFPSSalesContext, Order, Guid>, IOrdersTrendRepository
    {
        protected override List<Expression<Func<Order, object>>> DefaultIncludes => throw new NotImplementedException();

        public OrdersTrendRepository(IFPSSalesContext context) : base(context)
        {
        }

        public Task<List<OrderedFurnitureUnitsTrendsDbo>> GetOrdersGroupByFurnitureUnitsAggregated(int Take,DateTime? IntervalFrom, DateTime? IntervalTo)
        {
            var query = context.OrderedFurnitureUnits.AsQueryable();
            if (IntervalFrom.HasValue)
                query = query.Where(ent => (ent.OrderId != null && ent.Order.ContractDate >= IntervalFrom) || (ent.WebshopOrderId != null && ent.WebshopOrder.CreationTime >= IntervalFrom));
            if (IntervalTo.HasValue)
                query = query.Where(ent => (ent.OrderId != null && ent.Order.ContractDate <= IntervalTo) || (ent.WebshopOrderId != null && ent.WebshopOrder.CreationTime <= IntervalTo));

            return query
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(ent => ent.BaseFurnitureUnit)
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(ent => ent.Image)
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(ent => ent.Category)
                        .ThenInclude(ent=>ent.Translations)
                .Select(ent => new
                {
                    BaseFurnitureUnitId = ent.FurnitureUnit.BaseFurnitureUnitId == null ? ent.FurnitureUnitId : ent.FurnitureUnit.BaseFurnitureUnitId.Value,
                    Order = ent
                })
                .GroupBy(x => x.BaseFurnitureUnitId)
                .OrderByDescending(x=> x.Count())
                .Take(Take)
                .Select(x => new OrderedFurnitureUnitsTrendsDbo
                {
                    FurnitureUnitId = x.Key,
                    FurnitureUnit = x.First().Order.FurnitureUnit,
                    OrdersCount = x.Count(),
                })
                .ToListAsync();
        }

        public async Task<List<OrderedBoardMaterialsTrendDbo>> GetOrdersGroupByBoardMaterialsAggregated(FurnitureComponentTypeEnum FurnitureComponentType, int Take, DateTime? IntervalFrom, DateTime? IntervalTo)
        {
            var query = context.OrderedFurnitureUnits.Where(ent=> true==true);
            if (IntervalFrom.HasValue)
                query = query.Where(ent => (ent.OrderId != null && ent.Order.ContractDate >= IntervalFrom) || (ent.WebshopOrderId != null && ent.WebshopOrder.CreationTime >= IntervalFrom));
            if (IntervalTo.HasValue)
                query = query.Where(ent => (ent.OrderId != null && ent.Order.ContractDate <= IntervalTo) || (ent.WebshopOrderId != null && ent.WebshopOrder.CreationTime <= IntervalTo));

            var orderedMaterials = (await query
                .SelectMany(x => x.FurnitureUnit.Components)
                .Where(x => x.Type == FurnitureComponentType && x.BoardMaterialId.HasValue)
                .Select(x => x.BoardMaterial.Id)
                .GroupBy(x => x)
                .Select(x=>new {
                    MaterialIdGroup = x,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(Take)
                .ToListAsync());
            var materialsId = orderedMaterials.Select(gr => gr.MaterialIdGroup.Key).ToList();
            var materials= await context.Materials
                .Include(x => x.Category)
                    .ThenInclude(ent => ent.Translations)
                .Include(x=>x.Image)
                .Include(x=>x.Translations)
                .Where(x=> materialsId.Contains(x.Id))
                .Select(x=> (BoardMaterial)x)
                .ToListAsync();


            return orderedMaterials.Select(x => new OrderedBoardMaterialsTrendDbo
            {
                BoardMaterialId = x.MaterialIdGroup.Key,
                BoardMaterial = materials.First(m=> m.Id == x.MaterialIdGroup.Key),
                OrdersCount = x.Count,
            }).ToList();
        }

    }

}
