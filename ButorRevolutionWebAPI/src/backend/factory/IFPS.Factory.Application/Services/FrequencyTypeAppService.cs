using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class FrequencyTypeAppService : ApplicationService, IFrequencyTypeAppService
    {
        private readonly IFrequencyTypeRepository frequencyTypeRepository;

        public FrequencyTypeAppService(IApplicationServiceDependencyAggregate aggregate,
            IFrequencyTypeRepository frequencyTypeRepository)
            : base(aggregate)
        {
            this.frequencyTypeRepository = frequencyTypeRepository;
        }

        public async Task<List<FrequencyTypeListDto>> GetFrequencyTypesAsync()
        {
            var frequencyTypes = await frequencyTypeRepository.GetAllListAsync();

            return frequencyTypes.Select(ent => new FrequencyTypeListDto(ent)).ToList();
        }
    }
}
