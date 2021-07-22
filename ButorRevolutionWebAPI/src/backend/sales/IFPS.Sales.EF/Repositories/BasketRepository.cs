using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class BasketRepository : EFCoreRepositoryBase<IFPSSalesContext, Basket>, IBasketRepository
    {
        public BasketRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Basket, object>>> DefaultIncludes => new List<Expression<Func<Basket, object>>>
        {
        };

        public async Task<Basket> GetBasketAsync(int id)
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit.CurrentPrice.Price)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit.Category.Translations)
                .Include(ent => ent.SubTotal)
                    .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.DelieveryPrice)
                    .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Customer)
                    .ThenInclude(ent => ent.User)
                .SingleOrDefaultAsync(ent => ent.Id == id);
        }
    }
}
