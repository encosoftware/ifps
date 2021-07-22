using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;

namespace IFPS.Sales.Application.Services
{
    public class UserTeamTypeAppService : ApplicationService, IUserTeamTypeAppService
    {
        private readonly IUserTeamTypeRepository userTeamTypeRepository;

        public UserTeamTypeAppService(IApplicationServiceDependencyAggregate aggregate,
            IUserTeamTypeRepository userTeamTypeRepository) : base(aggregate)
        {
            this.userTeamTypeRepository = userTeamTypeRepository;
        }

        public async Task<List<UserTeamTypeListDto>> GetUserTeamTypesAsync()
        {
            return (await userTeamTypeRepository.GetAllListAsync()).Select(ent => new UserTeamTypeListDto(ent)).ToList();
        }
    }
}
