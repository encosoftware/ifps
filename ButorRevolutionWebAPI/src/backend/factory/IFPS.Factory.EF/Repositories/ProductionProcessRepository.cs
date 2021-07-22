using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class ProductionProcessRepository : EFCoreRepositoryBase<IFPSFactoryContext, ProductionProcess>, IProductionProcessRepository
    {
        public ProductionProcessRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<ProductionProcess, object>>> DefaultIncludes => new List<Expression<Func<ProductionProcess, object>>> { };

        public async Task<IPagedList<ProductionProcess>> GetPagedProductionProcessAsync
            (Type type,
            Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var processes = GetAll()
                .Include(ent => ent.Order)
                .Include(ent => ent.Plan.ConcreteFurnitureComponent.FurnitureComponent.BoardMaterial)
                .Include(ent => ent.Plan.WorkStation.Machine)
                .Include(ent => ent.Workers)
                    .ThenInclude(ent => ent.Worker)
                        .ThenInclude(worker => worker.CurrentVersion)
                .Include(ent => (ent.Plan as LayoutPlan).Board)
                .Where(ent => ent.Plan.GetType() == type);

            return await GetPagedListAsync(processes, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<IPagedList<ProductionProcess>> GetAssemblyManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var processes = GetAll()
                .Where(ent => ent.Plan.GetType() == typeof(ManualLaborPlan) &&
                            ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Assembly)
                .Include(ent => ent.Order)
                .Include(ent => (ent.Plan as ManualLaborPlan).ConcreteFurnitureUnit.FurnitureUnit)
                .Include(ent => ent.Workers)
                    .ThenInclude(ent => ent.Worker)
                        .ThenInclude(worker => worker.CurrentVersion);

            return await GetPagedListAsync(processes, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<IPagedList<ProductionProcess>> GetSortingManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var processes = GetAll()
                .Where(ent => ent.Plan.GetType() == typeof(ManualLaborPlan) && 
                            ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Sorting)
                .Include(ent => ent.Order)
                .Include(ent => (ent.Plan as ManualLaborPlan).ConcreteFurnitureUnit.FurnitureUnit)
                .Include(ent => ent.Workers)
                    .ThenInclude(ent => ent.Worker)
                        .ThenInclude(worker => worker.CurrentVersion);

            return await GetPagedListAsync(processes, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<IPagedList<ProductionProcess>> GetPackingManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var processes = GetAll()
                .Where(ent => ent.Plan.GetType() == typeof(ManualLaborPlan) &&
                            ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Packing)
                .Include(ent => ent.Order)
                .Include(ent => (ent.Plan as ManualLaborPlan).ConcreteFurnitureUnit.FurnitureUnit)
                .Include(ent => ent.Workers)
                    .ThenInclude(ent => ent.Worker)
                        .ThenInclude(worker => worker.CurrentVersion);

            return await GetPagedListAsync(processes, predicate, orderBy, pageIndex, pageSize);
        }
    }
}
