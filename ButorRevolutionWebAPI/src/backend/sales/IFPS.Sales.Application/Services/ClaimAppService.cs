using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class ClaimAppService : ApplicationService, IClaimAppService
    {

        private readonly IDivisionRepository divisionRepository;

        public ClaimAppService(IApplicationServiceDependencyAggregate aggregate,
            IDivisionRepository divisionRepository)
            : base(aggregate)
        {
            this.divisionRepository = divisionRepository;
        }

        public async Task<List<ClaimGroupDto>> GetClaimsWithGroupsList()
        {
            var divisions = await divisionRepository.GetAllWithClaimsTranslationAsync();
            return divisions.Select(ent => new ClaimGroupDto(ent)).ToList();
        }

        public async Task<List<string>> GetClaimsListAsync()
        {
            var divisions = await divisionRepository.GetAllWithClaimsTranslationAsync();
            var claims = new List<string>();
            foreach (var division in divisions)
            {
                claims.AddRange(division.Claims.Select(ent => ent.Name.ToString()).ToList());
            }
            return claims;
        }
    }
}
