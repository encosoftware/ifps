using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
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
    public class InspectionRepository : EFCoreRepositoryBase<IFPSFactoryContext, Inspection>, IInspectionRepository
    {
        public InspectionRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Inspection, object>>> DefaultIncludes => new List<Expression<Func<Inspection, object>>>
        {

        };

        public Task<Inspection> GetInspectionReportWithIncludesAsync(int id)
        {
            return GetAll()
                .Include(ent=> ent.InspectedStorage)
                .Include(ent => ent.Report)
                    .ThenInclude(ent => ent.StockReports)
                        .ThenInclude(ent => ent.Stock)
                            .ThenInclude(ent => ent.Package)
                .Include(ent => ent.Report)
                    .ThenInclude(ent => ent.StockReports)
                        .ThenInclude(ent => ent.Stock)
                            .ThenInclude(ent => ent.StorageCell)
                .Include(ent => ent.UserInspections)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .SingleAsync(ent => ent.Id == id);
        }
    }
}
