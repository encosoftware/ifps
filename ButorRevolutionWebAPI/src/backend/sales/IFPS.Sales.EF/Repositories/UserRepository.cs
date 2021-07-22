using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
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
    public class UserRepository : EFCoreRepositoryBase<IFPSSalesContext, User>, IUserRepository
    {
        public UserRepository(
            IFPSSalesContext context
            ) : base(context)
        {
        }

        protected override List<Expression<Func<User, object>>> DefaultIncludes => new List<Expression<Func<User, object>>>
        {
        };

        public Task<List<User>> GetUsersWithDataRoleAsync()
        {
            return context.Users
                .Include(u => u.Image)
                .Include(u => u.Roles)
                    .ThenInclude(r => r.Role)
                .ToListAsync();
        }

        public async Task<User> GetUserWithDataRoleClaimsAsync(int id)
        {
            var result = await context.Users
                .Include(u => u.Image)
                .Include(u => u.CurrentVersion)
                .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(d => d.Division)
                .Include(u => u.Company)
                .Include(u => u.Claims)
                    .ThenInclude(uc => uc.Claim)
                        .ThenInclude(c => c.Division)
                .SingleOrDefaultAsync(u => u.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(User), id);
        }

        public async Task<User> GetUserWithClaimsAsync(int id)
        {
            var result = await context.Users
                .Include(u => u.Claims)
                    .ThenInclude(uc => uc.Claim)
                        .ThenInclude(c => c.Division)
                .SingleOrDefaultAsync(u => u.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(User), id);
        }

        public async Task<User> GetUserWithClaimsAndTokenByNameAsync(string userName)
        {
            var result = await context.Users
                .Include(u => u.Roles)
                    .ThenInclude(ent => ent.Role)
                .Include(u => u.Image)
                .Include(u => u.CurrentVersion)
                .Include(u => u.Tokens)
                .Include(u => u.Claims)
                    .ThenInclude(uc => uc.Claim)
                        .ThenInclude(c => c.Division)
                .SingleOrDefaultAsync(u => u.CurrentVersion.Name == userName || u.Email == userName);

            return result ?? throw new EntityNotFoundException(typeof(User), userName);
        }

        public Task<List<User>> GetUsersWithDataAsync(List<int> userIds)
        {
            return context.Users
                .Include(u => u.Image)
                .Include(u => u.CurrentVersion)
                .Where(u => userIds.Any(id => u.Id == id))
                .ToListAsync();
        }

        public async Task<IPagedList<TResult>> GetUsersPagedWithDataRoleAsync<TResult>(
            Expression<Func<User, bool>> predicate,
            Expression<Func<User, TResult>> selector,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, int pageIndex = 0, int pageSize = 20)
            where TResult : class
        {
            var query = GetAll();

            return await GetPagedListAsync(query, predicate, selector, orderBy, pageIndex, pageSize);
        }

        public Task<List<User>> GetUsersWithDataAsync(Expression<Func<User, bool>> predicate, int? userNumber = null)
        {
            var query = context.Users
                .Include(u => u.Image)
                .Include(u => u.CurrentVersion)
                .Where(predicate);

            if (userNumber.HasValue)
                query = query.Take(userNumber.Value);

            return query.ToListAsync();
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            var count = await context.Users
                .Where(u => u.Email.Equals(email))
                .CountAsync();

            return count > 0;
        }

        public Task<List<User>> GetUsersWithRolesAndCurrentVersionAsync(Expression<Func<User, bool>> predicate)
        {
            return GetAll()
                .Include(ent => ent.CurrentVersion)
                .Include(ent => ent.Roles)
                    .ThenInclude(ent => ent.Role)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<User> GetUserByCreateOrderByUserIdAsync(int userId)
        {
            return await GetAll()
                .Include(ent => ent.CurrentVersion)
                .SingleAsync(ent => ent.Id == userId);
        }

        public async Task<User> GetUserDivisionAsync(int userId)
        {
            return await GetAll()
                .Include(ent => ent.Roles)
                    .ThenInclude(userRole => userRole.Role)
                        .ThenInclude(role => role.Division)
                .SingleAsync(ent => ent.Id == userId);
        }
    }
}