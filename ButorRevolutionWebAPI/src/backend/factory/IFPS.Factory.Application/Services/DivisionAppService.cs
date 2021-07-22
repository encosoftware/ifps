using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class DivisionAppService : ApplicationService, IDivionAppService
    {

        private readonly IDivisionRepository divisionRepository;

        public DivisionAppService(IApplicationServiceDependencyAggregate aggregate,
            IDivisionRepository divisionRepository)
            : base(aggregate)
        {
            this.divisionRepository = divisionRepository;
        }

        public async Task<List<DivisionClaimsListDto>> GetDivisionClaimsListAsync()
        {
            var divisions = await divisionRepository.GetAllWithClaimsTranslationAsync();

            return divisions.Select(ent => new DivisionClaimsListDto(ent)).ToList();
        }

        public async Task<List<DivisionListDto>> GetDivisionListAsync()
        {
            var divisions = await divisionRepository.GetAllWithClaimsTranslationAsync();

            return divisions.Select(ent => new DivisionListDto(ent)).ToList();
        }
    }
}
