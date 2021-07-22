using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Enums;
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
    public class CompanyRepository : EFCoreRepositoryBase<IFPSFactoryContext, Company>, ICompanyRepository
    {
        public CompanyRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<Company, object>>> DefaultIncludes => new List<Expression<Func<Company, object>>>
        {
        };

        public async Task<IPagedList<Company>> GetCompaniesWithIncludesAsync(Expression<Func<Company, bool>> predicate = null,
            Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentVersion)
                    .ThenInclude(ent => ent.ContactPerson)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CompanyType);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await GetAll()
                .Include(ent => ent.CurrentVersion)
                    .ThenInclude(ent => ent.ContactPerson)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.UserTeams)
                    .ThenInclude(ut => ut.TechnicalUser)
                        .ThenInclude(ut => ut.CurrentVersion)
                .Include(ent => ent.UserTeams)
                    .ThenInclude(ut => ut.User)
                        .ThenInclude(u => u.CurrentVersion)
                .Include(ent => ent.CompanyType)
                .Include(ent => ent.OpeningHours)
                .SingleAsync(ent => ent.Id == id)

                ?? throw new InvalidOperationException();
        }

        public async Task<List<Company>> GetSuppliersListAsync()
        {
            return await GetAll()
                .Include(ent => ent.CompanyType) 
                .GroupBy(ent => ent.Name)
                .Select(g => g.FirstOrDefault())
                .Where(ent => ent.CompanyType.Type == CompanyTypeEnum.SupplierCompany)
                .ToListAsync();
        }
    }
}
