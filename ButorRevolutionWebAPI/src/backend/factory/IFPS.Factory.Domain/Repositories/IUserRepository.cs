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
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsersWithDataRoleAsync();

        Task<IPagedList<User>> GetUsersPagedWithDataRoleAsync(
            Expression<Func<User, bool>> predicate,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy,
             int pageIndex = 0, int pageSize = 20); // where TResult : class;

        Task<User> GetUserWithDataRoleClaimsAsync(int id);
        Task<User> GetUserWithClaimsAsync(int id);

        Task<List<User>> GetUsersWithDataAsync(List<int> userIds);

        Task<List<User>> GetUsersWithDataAsync(Expression<Func<User, bool>> predicate, int? take = null);

        Task<bool> IsUserExistsAsync(string email);

        Task<User> GetUserWithClaimsAndTokenByNameAsync(string name);
        Task<List<User>> GetBookedByForDropdownListAsync();
    }
}
