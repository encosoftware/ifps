using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class UserRepository : EFCoreRepositoryBase<IFPSFactoryContext, User>, IUserRepository
    {
        public UserRepository(IFPSFactoryContext context) : base(context)
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
                .Include(ent => ent.CurrentVersion)
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

        public async Task<IPagedList<User>> GetUsersPagedWithDataRoleAsync(
            Expression<Func<User, bool>> predicate,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy,
             int pageIndex = 0, int pageSize = 20)
        {
            var query = GetAll()
              .Include(ent => ent.CurrentVersion)
              .Include(ent => ent.Company)

              .Include(ent => ent.Roles)
                .ThenInclude(ent => ent.Role)
                    .ThenInclude(ent => ent.Translations);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public Task<List<User>> GetUsersWithDataAsync(Expression<Func<User, bool>> predicate, int? take = null)
        {
            var query = context.Users
                .Include(u => u.Image)
                .Include(u => u.CurrentVersion)
                .Where(predicate);

            if (take.HasValue)
                query.Take(take.Value);

            return query.ToListAsync();
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            var count = await context.Users
                .Where(u => u.Email.Equals(email))
                .CountAsync();

            return count > 0;
        }

        public async Task<List<User>> GetBookedByForDropdownListAsync()
        {
            return await GetAll()
                .Include(ent => ent.CurrentVersion)
                .GroupBy(ent => ent.CurrentVersion.Name)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();
        }
    }
}
