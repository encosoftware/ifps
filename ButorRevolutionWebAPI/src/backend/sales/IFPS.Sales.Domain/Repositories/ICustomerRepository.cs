using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByUserIdWithNofiNotificationModeAsync(int userId);
        Task<Customer> GetByUserIdAsync(int userId);
        Task<bool> IsCustomerExistsAsync(int userId);
        Task<Customer> GetRecommendedProductsByCustomerId(int customerId);        
    }
}
