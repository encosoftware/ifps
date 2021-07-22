using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class SortingStrategyTypesAppService : ApplicationService, ISortingStrategyTypesAppService
    {
        private readonly ISortingStrategyTypeRepository sortingStrategyTypeRepository;

        public SortingStrategyTypesAppService(
            IApplicationServiceDependencyAggregate aggregate, 
            ISortingStrategyTypeRepository sortingStrategyTypeRepository) : base(aggregate)
        {
            this.sortingStrategyTypeRepository = sortingStrategyTypeRepository;
        }

        public async Task<List<SortingStrategyTypeListDto>> GetSortingStrategyTypesAsync()
        {
            var strategyTypes = await sortingStrategyTypeRepository.GetAllListIncludingAsync(ent => true, ent => ent.Translations);
            return strategyTypes.Select(ent => new SortingStrategyTypeListDto(ent)).ToList();
        }
    }
}
