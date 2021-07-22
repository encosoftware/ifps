using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
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
    public class SalesPersonRepository : EFCoreRepositoryBase<IFPSSalesContext, SalesPerson>, ISalesPersonRepository
    {
        public SalesPersonRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<SalesPerson, object>>> DefaultIncludes => new List<Expression<Func<SalesPerson, object>>>
        {
            
        };
        public async Task<SalesPerson> GetByUserIdAsync(int userId)
        {
            var result = await context.SalesPersons                
                .SingleOrDefaultAsync(sp => !sp.ValidTo.HasValue && sp.UserId == userId);

            return result ?? throw new EntityNotFoundException(typeof(SalesPerson), userId);
        }

        public async Task<SalesPerson> GetByUserIdWithOfficesSupervisorsSuperviseesAsync(int userId)
        {
            var result = await context.SalesPersons
                .Include(sp => sp.Offices)
                    .ThenInclude(o => o.Office)
                .Include(sp => sp.Supervisor)                    
                .Include(sp => sp.Supervisees)
                    .ThenInclude(s=>s.User)
                        .ThenInclude(u=>u.CurrentVersion)
                .Include(sp => sp.DefaultTimeTable)
                    .ThenInclude(dtt => dtt.DayType)
                        .ThenInclude(dtt => dtt.Translations)
                .Include(sp => sp.DefaultTimeTable)
                    .ThenInclude(dtt => dtt.Interval)                
                .SingleOrDefaultAsync(sp => !sp.ValidTo.HasValue && sp.UserId == userId) 
                ?? throw new EntityNotFoundException(typeof(SalesPerson), userId);
            
            if (result.SupervisorId.HasValue)
                await context.Entry(result.Supervisor)
                    .Reference(sv=>sv.User)
                    .Query()
                        .Include(s => s.CurrentVersion)
                    .LoadAsync();

            return result;
        }

        public Task<List<SalesPerson>> GetSalesPersonsByIdsAsync(List<int> ids)
        {
            return context.SalesPersons                            
                            .Where(sp => !sp.ValidTo.HasValue && ids.Any(id => sp.Id == id))
                            .ToListAsync();                            
        }

        public Task<bool> IsSalesPersonExistsAsync(int userId)
        {
            return context.SalesPersons                
                .AnyAsync(sp => !sp.ValidTo.HasValue && sp.UserId == userId);
        }
        
    }
}
