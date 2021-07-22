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
    public class CompanyRepository : EFCoreRepositoryBase<IFPSSalesContext, Company>, ICompanyRepository
    {
        public CompanyRepository(IFPSSalesContext context) : base(context)
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

        public async Task<Company> GetCompanyAsync(int id)
        {
            var result = await GetAll()
               .Include(ent => ent.CurrentVersion)
                   .ThenInclude(ent => ent.ContactPerson)
                       .ThenInclude(ent => ent.CurrentVersion)
               .Include(ent => ent.UserTeams)
                   .ThenInclude(ut => ut.TechnicalUser)
                       .ThenInclude(ut => ut.CurrentVersion)
               .Include(ent => ent.UserTeams)
                   .ThenInclude(ut => ut.User)
                       .ThenInclude(u => u.CurrentVersion)
                .Include(ent => ent.UserTeams)
                   .ThenInclude(ut => ut.UserTeamType)
                        .ThenInclude(utt => utt.Translations)
               .Include(ent => ent.CompanyType)
               .Include(ent => ent.OpeningHours)
               .SingleAsync(ent => ent.Id == id);

            return result ?? throw new EntityNotFoundException(typeof(Company), id);
        }
    }
}
