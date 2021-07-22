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
    public class CustomerRepository : EFCoreRepositoryBase<IFPSSalesContext, Customer>, ICustomerRepository
    {
        public CustomerRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Customer, object>>> DefaultIncludes => new List<Expression<Func<Customer, object>>>
        {
        };
        public async Task<Customer> GetByUserIdAsync(int userId)
        {
           return await GetAll().SingleOrDefaultAsync(c => !c.ValidTo.HasValue && c.UserId == userId) ?? throw new EntityNotFoundException(typeof(Customer), userId);
        }

        public async Task<Customer> GetByUserIdWithNofiNotificationModeAsync(int userId)
        {
            var result = await GetAll()
                .Include(c => c.NotificationModes)
                    .ThenInclude(nm => nm.EventType)
                        .ThenInclude(et => et.Translations)
                 .Where(c => c.ValidFrom <= Clock.Now && (!c.ValidTo.HasValue || c.ValidTo.Value >= Clock.Now))
                 .SingleOrDefaultAsync(c => c.UserId == userId) 
                 ?? throw new EntityNotFoundException(typeof(Customer), userId);

            if (result.NotificationModes.Any() && result.NotificationModes.Select(nm => nm.EventType).Any(et => et.Translations.Any()))
                await context.Entry(result)
                    .Collection(c => c.NotificationModes)
                    .Query()
                    .Include(nm => nm.EventType)
                        .ThenInclude(et => et.Translations)
                    .LoadAsync();

            return result;
        }

        public async Task<Customer> GetRecommendedProductsByCustomerId(int customerId)
        {
            return await GetAll()
                .Include(x => x.RecommendedProducts)
                    .ThenInclude(x => x.WebshopFurnitureUnit)
                        .ThenInclude(x => x.FurnitureUnit)
                            .ThenInclude(x => x.Image)

                .Include(x => x.RecommendedProducts)
                    .ThenInclude(x => x.WebshopFurnitureUnit)
                        .ThenInclude(x => x.Price)
                                .ThenInclude(x => x.Currency)

                //.SingleAsync(x => x.Id == customerId) ?? throw new EntityNotFoundException(typeof(Customer), customerId);
                .SingleOrDefaultAsync(x => x.Id == customerId) ?? throw new EntityNotFoundException(typeof(Customer), customerId);
        }

        public Task<bool> IsCustomerExistsAsync(int userId)
        {
            return context.Customers.AnyAsync(c => !c.ValidTo.HasValue && c.UserId == userId);
        }
    }
}
