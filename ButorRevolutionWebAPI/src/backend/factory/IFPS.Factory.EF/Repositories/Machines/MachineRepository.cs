using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.Paging;

namespace IFPS.Factory.EF.Repositories
{
    public class MachineRepository : EFCoreRepositoryBase<IFPSFactoryContext, Machine>, IMachineRepository
    {
        public MachineRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Machine, object>>> DefaultIncludes => new List<Expression<Func<Machine, object>>>
        {

        };

        public async Task<List<Machine>> GetMachinesByType(Type type)
        {
            return await GetAll()
                .Where(ent => ent.GetType() == type)
                .ToListAsync();
        }

        public async Task<IPagedList<Machine>> GetMachinesAsync(Expression<Func<Machine, bool>> predicate = null, Func<IQueryable<Machine>, IOrderedQueryable<Machine>> orderBy = null, int pageIndex = 0, int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.Brand)
                    .ThenInclude(ent => ent.CompanyType);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}
