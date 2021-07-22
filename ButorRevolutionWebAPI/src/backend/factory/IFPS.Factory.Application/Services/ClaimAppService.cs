using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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

        public async Task<List<ClaimGroupDto>> GetClaimsWithGroupsListAsync()
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
