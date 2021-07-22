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
    public class EmployeeRepository : EFCoreRepositoryBase<IFPSSalesContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Employee, object>>> DefaultIncludes => new List<Expression<Func<Employee, object>>>
        {
            
        };

        public async Task<List<EmployeeAbsenceDay>> GetAbsenceDaysByUserIdAsync(int userId, DateRange range)
        {
            var result = await context.Employees
                .Include(e => e.AbsenceDays)                
                .Where(e => !e.ValidTo.HasValue && e.UserId == userId)
                .SelectMany(e => e.AbsenceDays)
                .ToListAsync();

            return result;
        }

        public async Task<Employee> GetByUserIdAsync(int userId)
        {
            var result = await context.Employees                
                .SingleOrDefaultAsync(e => !e.ValidTo.HasValue && e.UserId == userId);

            return result ?? throw new EntityNotFoundException(typeof(Employee), userId);
        }

        public async Task<Employee> GetByUserIdWithAbsenceDaysAsync(int userId)
        {
            var result = await context.Employees
                .Include(e => e.AbsenceDays)                
                .SingleOrDefaultAsync(e => e.UserId == userId && !e.ValidTo.HasValue);

            return result ?? throw new EntityNotFoundException(typeof(Employee), userId);
        }

        public Task<bool> IsEmployeeExistsAsync(int userId)
        {
            return context.Employees
                .AnyAsync(e => !e.ValidTo.HasValue && e.UserId == userId);
        }
    }
}
