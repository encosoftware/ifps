using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CuttingsAppService : ApplicationService, ICuttingsAppService
    {
        private readonly IProductionProcessRepository productionProcessRepository;

        public CuttingsAppService(IApplicationServiceDependencyAggregate aggregate,
            IProductionProcessRepository productionProcessRepository
            ) : base(aggregate)
        {
            this.productionProcessRepository = productionProcessRepository;
        }

        public async Task<PagedListDto<CuttingsListDto>> CuttingsListAsync(CuttingsFilterDto filterDto)
        {
            Expression<Func<ProductionProcess, bool>> filter = (ProductionProcess c) => c.Plan != null;

            if (!string.IsNullOrEmpty(filterDto.Machine))
            {
                filter = filter.And(ent => ent.Plan.WorkStation.Machine.Name.ToLower().Contains(filterDto.Machine.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.MaterialCode))
            {
                filter = filter.And(ent => ent.Plan.ConcreteFurnitureComponent.FurnitureComponent.BoardMaterial.Code.ToLower().Contains(filterDto.MaterialCode.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.OrderName))
            {
                filter = filter.And(ent => ent.Order.OrderName.ToLower().Contains(filterDto.OrderName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.WorkingNumber))
            {
                filter = filter.And(ent => ent.Order.WorkingNumber.ToLower().Contains(filterDto.WorkingNumber.ToLower().Trim()));
            }
            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.Plan.ScheduledStartTime >= filterDto.From);
            }
            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.Plan.ScheduledStartTime <= filterDto.To);
            }
            if (!string.IsNullOrEmpty(filterDto.WorkerName))
            {
                filter = filter.And(ent => ent.Workers.Any(e => e.Worker.CurrentVersion.Name.ToLower().Contains(filterDto.WorkerName.ToLower().Trim())));
            }


            var orderingQuery = filterDto.Orderings.ToOrderingExpression<ProductionProcess>(CuttingsFilterDto.GetOrderingMapping(), nameof(ProductionProcess.Id));

            var allProductionProcess = await productionProcessRepository.GetPagedProductionProcessAsync(typeof(LayoutPlan), filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return allProductionProcess.ToPagedList(CuttingsListDto.FromEntity);
        }
    }
}
