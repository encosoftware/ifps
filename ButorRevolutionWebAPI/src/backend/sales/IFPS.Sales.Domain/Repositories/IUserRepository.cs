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
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsersWithDataRoleAsync();
        Task<IPagedList<TResult>> GetUsersPagedWithDataRoleAsync<TResult>(
            Expression<Func<User, bool>> predicate = null,
            Expression<Func<User, TResult>> selector = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20) where TResult : class;
        Task<User> GetUserWithDataRoleClaimsAsync(int id);
        Task<User> GetUserWithClaimsAsync(int id);
        Task<List<User>> GetUsersWithDataAsync(List<int> userIds);
        Task<List<User>> GetUsersWithDataAsync(Expression<Func<User, bool>> predicate, int? userNumber = null);
        Task<bool> IsUserExistsAsync(string email);

        Task<User> GetUserWithClaimsAndTokenByNameAsync(string userName);
        Task<List<User>> GetUsersWithRolesAndCurrentVersionAsync(Expression<Func<User, bool>> predicate);
        Task<User> GetUserByCreateOrderByUserIdAsync(int userId);
        Task<User> GetUserDivisionAsync(int userId);
    }
}
