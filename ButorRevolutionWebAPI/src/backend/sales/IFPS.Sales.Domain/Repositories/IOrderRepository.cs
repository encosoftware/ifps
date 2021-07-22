using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IPagedList<Order>> GetOrders(Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<IPagedList<Order>> GetOrdersByCompany(int companyId, Expression<Func<Order, bool>> predicate = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderWithTicketsAsync(Guid id);
        Task<Order> GetCustomerOrderWithTicketsAsync(Guid id, int customerId);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> GetOrderForMessagesAsync(Guid id);
        Task<int> GetNextWorkingNumber(int year);
        Task<Order> GetOrderWithIncludesById(Guid orderId);
        Task<Order> GetOrderWithIncludesForListById(Guid orderId);
        Task<Order> GetOrderWithIncludesForOrderedFurnitureUnitDetailsById(Guid orderId);
        Task<Order> GetOrderForCreateFurnitureUnitById(Guid orderId);
        Task<List<Order>> GetOrderWithOrderedFurnitureUnits(Expression<Func<Order, bool>> predicate = null);
        Task<List<TResult>> GetDocumentGroupsAsync<TResult>(Guid orderId, Expression<Func<DocumentGroup, TResult>> selector);
        Task<DocumentGroup> GetDocumentGroupWithFolderAsync(Guid orderId, int documentGroupId);
        Task<DocumentGroupVersion> GetDocumentGroupVersionWithIncludesAsync(Guid orderId, int documentGroupVersionId);
        Task<Order> GetOrderForContractById(Guid orderId);
        Task<OrdersDbo> GetOrderByIdAsync(Expression<Func<Order, bool>> predicate, Expression<Func<Order, OrdersDbo>> selector);
        Task<Order> GetOrderWithOrderedAppliances(Guid id);
        Task<Order> GetOrderByIdByAddService(Guid id);
        Task<Order> GetOrderDeleteByIdAsync(Guid orderId);
        Task<bool> IsOrderWithSalesPersonExistingAsync(int salesPersonId);
        Task<bool> IsOrderWithCustomerExistingAsync(int customerId);
    }
}
