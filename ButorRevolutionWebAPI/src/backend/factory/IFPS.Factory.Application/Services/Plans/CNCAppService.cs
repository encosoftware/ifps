using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CNCAppService : ApplicationService, ICncAppService
    {
        private readonly IProductionProcessRepository productionProcessRepository;

        public CNCAppService(IApplicationServiceDependencyAggregate aggregate,
            IProductionProcessRepository productionProcessRepository
            ) : base(aggregate)
        {
            this.productionProcessRepository = productionProcessRepository;
        }

        public async Task<PagedListDto<CNCListDto>> CncListAsync(CncFilterDto filterDto)
        {
            Expression<Func<ProductionProcess, bool>> filter = (ProductionProcess c) => c.Plan != null;
            filter = filter.And(ent => ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Cnc);

            if (!string.IsNullOrEmpty(filterDto.ComponentName))
            {
                filter = filter.And(ent => ent.Plan.ConcreteFurnitureComponent.FurnitureComponent.Name.ToLower().Contains(filterDto.ComponentName.ToLower().Trim()));
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


            var orderingQuery = filterDto.Orderings.ToOrderingExpression<ProductionProcess>(CncFilterDto.GetOrderingMapping(), nameof(ProductionProcess.Id));

            var allProductionProcess = await productionProcessRepository.GetPagedProductionProcessAsync(typeof(CncPlan), filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return allProductionProcess.ToPagedList(CNCListDto.FromEntity);
        }
    }
}
