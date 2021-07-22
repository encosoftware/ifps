using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IPagedList<Order>> GetPagedOrderSchedulingAsync
            (Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<IPagedList<Order>> GetOrdersByCompany(int companyId, Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<Order> GetOrderWithTicketsAsync(Guid id);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetAllOrdersWithSomeIncludes();
        Task<Order> GetOrderByIdForCutting(Guid id);
        Task<Order> GetOrderByWithConcretes(Guid orderId);
        Task<Order> GetOrderByIdForMaterialReservation(Guid orderId);
        Task<int> GetOldestYear();
    }
}
