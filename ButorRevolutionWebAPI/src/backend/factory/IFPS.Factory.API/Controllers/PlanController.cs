using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/plans")]
    [ApiController]
    public class PlanController : IFPSControllerBase
    {
        private const string OPNAME = "Plans";
        private readonly IPlanAppService planAppService;

        public PlanController(IPlanAppService planAppService)
        {
            this.planAppService = planAppService;
        }

        [HttpPut("{ipAddress}/{cfcId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetPlanProductionProcessTime(string ipAddress, int cfcId)
        {
            return planAppService.SetPlanProductionProcessTimeAsync(ipAddress, cfcId);
        }
    }
}
