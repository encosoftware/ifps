using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IWebshopOrderRepository : IRepository<WebshopOrder, Guid>
    {
        Task<List<WebshopOrder>> GetOrderWithOrderedFurnitureUnits(Expression<Func<WebshopOrder, bool>> predicate = null);
        Task<int> GetNextWorkingNumber(int year);
        Task<WebshopOrder> GetWebshopOrderWithIncludesById(Guid webshopOrderId);
        Task<List<List<string>>> GetOrderedFUCodes();
    }
}
