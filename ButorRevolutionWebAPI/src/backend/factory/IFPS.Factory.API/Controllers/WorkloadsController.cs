using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/workloads")]
    [ApiController]
    public class WorkloadsController : IFPSControllerBase
    {
        private const string OPNAME = "Workloads";
        private readonly IWorkStationsAppService workStationsAppService;

        public WorkloadsController(IWorkStationsAppService workStationsAppService)
        {
            this.workStationsAppService = workStationsAppService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WorkStationsPlansListDto> GetWorkStationsPlans()
        {
            return workStationsAppService.GetWorkStationsByWorkloadPageAsync();
        }
    }
}
