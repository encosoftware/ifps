using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface ISalesPersonRepository : IRepository<SalesPerson>
    {
        Task<SalesPerson> GetByUserIdWithOfficesSupervisorsSuperviseesAsync(int userId);
        Task<SalesPerson> GetByUserIdAsync(int userId);
        Task<List<SalesPerson>> GetSalesPersonsByIdsAsync(List<int> ids);
        Task<bool> IsSalesPersonExistsAsync(int userId);
    }
}
