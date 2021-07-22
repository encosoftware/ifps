using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
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
    public class WorkStationRepository : EFCoreRepositoryBase<IFPSFactoryContext, WorkStation>, IWorkStationRepository
    {
        public WorkStationRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<WorkStation, object>>> DefaultIncludes => new List<Expression<Func<WorkStation, object>>>
        {
            
        };

        public async Task<IPagedList<WorkStation>> GetPagedWorkStationsAsync(
            Expression<Func<WorkStation, bool>> predicate = null, 
            Func<IQueryable<WorkStation>, IOrderedQueryable<WorkStation>> orderBy = null, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var workStations = GetAll()
                .Include(ent => ent.WorkStationType)
                    .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Machine);

            return await GetPagedListAsync(workStations, predicate, orderBy, pageIndex, pageSize);
        }

        public Task<WorkStation> GetWorkStationsWithCameras(int workStationId)
        {
            return GetAll()
                .Include(ent => ent.WorkStationCameras)
                .ThenInclude(ent => ent.CFCProductionState)
                .SingleAsync(ent=> ent.Id == workStationId);
        }
    }
}
