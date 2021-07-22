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
    public class AssemblyAppService : ApplicationService, IAssemblyAppService
    {
        private readonly IProductionProcessRepository productionProcessRepository;

        public AssemblyAppService(IApplicationServiceDependencyAggregate aggregate,
            IProductionProcessRepository productionProcessRepository
            ) : base(aggregate)
        {
            this.productionProcessRepository = productionProcessRepository;
        }

        public async Task<PagedListDto<AssemblyListDto>> AssemblyListAsync(AssemblyFilterDto filterDto)
        {
            Expression<Func<ProductionProcess, bool>> filter = (ProductionProcess c) => c.Plan != null;

            if (!string.IsNullOrEmpty(filterDto.UnitName))
            {
                filter = filter.And(ent => ent.Plan.ConcreteFurnitureUnit.FurnitureUnit.Code.ToLower().Contains(filterDto.UnitName.ToLower().Trim()));
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

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<ProductionProcess>(AssemblyFilterDto.GetOrderingMapping(), nameof(ProductionProcess.Id));

            var allProductionProcess = await productionProcessRepository.GetAssemblyManualProductionProcessesAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return allProductionProcess.ToPagedList(AssemblyListDto.FromEntity);
        }
    }
}
