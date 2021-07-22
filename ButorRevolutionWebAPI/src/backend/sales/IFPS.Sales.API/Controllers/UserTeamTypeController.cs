using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/userteamtypes")]
    [ApiController]
    public class UserTeamTypeController : IFPSControllerBase
    {
        private readonly IUserTeamTypeAppService userTeamTypeAppService;
        private const string OPNAME = "UserTeamTypes";

        public UserTeamTypeController(IUserTeamTypeAppService userTeamTypeAppService)
        {
            this.userTeamTypeAppService = userTeamTypeAppService;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<UserTeamTypeListDto>> GetUserTeamTypes()
        {
            return this.userTeamTypeAppService.GetUserTeamTypesAsync();
        }
    }
}
