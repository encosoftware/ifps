using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class CFCProductionStatesAppService : ApplicationService, ICFCProductionStatesAppService
    {
        private readonly ICFCProductionStateRepository cfcProductionStateRepository;

        public CFCProductionStatesAppService(
            ICFCProductionStateRepository cfcProductionStateRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.cfcProductionStateRepository = cfcProductionStateRepository;
        }

        public async Task<List<CFCProductionStateListDto>> GetCFCProductionStatesAsync()
        {
            var cfcProductionStates = await cfcProductionStateRepository.GetAllListAsync();
            return cfcProductionStates.Select(ent => new CFCProductionStateListDto(ent)).ToList();
        }
    }
}
