using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
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
    public class WebshopOrderRepository : EFCoreRepositoryBase<IFPSSalesContext, WebshopOrder, Guid>, IWebshopOrderRepository
    {
        public WebshopOrderRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<WebshopOrder, object>>> DefaultIncludes => new List<Expression<Func<WebshopOrder, object>>>
        { };

        public async Task<List<WebshopOrder>> GetOrderWithOrderedFurnitureUnits(Expression<Func<WebshopOrder, bool>> predicate = null)
        {
            return await GetAll()
                .Where(predicate)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit.Components)
                .Include(ent => ent.Basket.SubTotal)
                .ToListAsync();
        }

        public async Task<int> GetNextWorkingNumber(int year)
        {
            var max = await GetAll()
                .Where(ent => ent.WorkingNumberYear == year)
                .Select(ent => ent.WorkingNumberSerial)
                .DefaultIfEmpty(0)
                .MaxAsync();

            return max + 1;
        }

        public async Task<WebshopOrder> GetWebshopOrderWithIncludesById(Guid webshopOrderId)
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit)
                        .ThenInclude(fu => fu.Image)
                .Include(ofu => ofu.Basket)
                        .ThenInclude(basket => basket.SubTotal)
                            .ThenInclude(subtotal => subtotal.Currency)
                .Include(ofu => ofu.Basket)
                        .ThenInclude(basket => basket.DelieveryPrice)
                            .ThenInclude(subtotal => subtotal.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.Basket)
                        .ThenInclude(basket => basket.SubTotal)
                            .ThenInclude(subtotal => subtotal.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.Basket)
                        .ThenInclude(basket => basket.DelieveryPrice)
                            .ThenInclude(delivery => delivery.Currency)
                .SingleAsync(ent => ent.Id == webshopOrderId);
        }

        public async Task<List<List<string>>> GetOrderedFUCodes()
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit)
                .Select(wo => wo.OrderedFurnitureUnits
                    .Select(ofu => ofu.FurnitureUnit.Code).ToList())
                .ToListAsync();
        }
    }
}
