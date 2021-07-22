using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/cfcproductionstates")]
    [ApiController]
    public class CFCProductionStateController : IFPSControllerBase
    {
        private const string OPNAME = "CFCProductionStates";
        private readonly ICFCProductionStatesAppService cfcProductionStatesAppService;

        public CFCProductionStateController(ICFCProductionStatesAppService cfcProductionStatesAppService)
        {
            this.cfcProductionStatesAppService = cfcProductionStatesAppService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<CFCProductionStateListDto>> GetCFCProductionStates()
        {
            return cfcProductionStatesAppService.GetCFCProductionStatesAsync();
        }
    }
}
