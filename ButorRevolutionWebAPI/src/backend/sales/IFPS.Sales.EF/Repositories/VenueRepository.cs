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
    public class VenueRepository : EFCoreRepositoryBase<IFPSSalesContext, Venue>, IVenueRepository
    {
        public VenueRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Venue, object>>> DefaultIncludes => new List<Expression<Func<Venue, object>>>
        {
        };

        public async Task<IPagedList<Venue>> GetPagedVenuesAsync(Expression<Func<Venue, bool>> predicate = null, Func<IQueryable<Venue>, IOrderedQueryable<Venue>> orderBy = null, int pageIndex = 0, int pageSize = 20)
        {
            var venue = GetAll()
                .Include(i => i.MeetingRooms)
                .Include(i => i.OpeningHours);

            return await GetPagedListAsync(venue, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<Venue> GetVenueAsync(int venueId)
        {
            var result = await GetAll()
                .Include(entity => entity.OpeningHours)
                    .ThenInclude(ent => ent.DayType)
                .Include(entity => entity.MeetingRooms)
                .SingleOrDefaultAsync(entity => entity.Id == venueId);

            return result ?? throw new EntityNotFoundException(typeof(Venue), venueId);
        }

        public Task<List<Venue>> GetVenuesByCompanyAsync(int companyId)
        {
            return GetAll()
                .Where(ent => ent.CompanyId == companyId)
                .Include(ent => ent.MeetingRooms)
                    .ThenInclude(ent => ent.Appointments)
                .ToListAsync();
        }

        public Task<List<Venue>> GetVenuesAsync(Expression<Func<Venue, bool>> predicate, int take = 10)
        {
            var query = GetAll()
                .Include(ent => ent.MeetingRooms)
                .Include(ent => ent.OpeningHours)
                .Where(predicate);

            query.Take(take);

            return query.ToListAsync();
        }
    }
}
