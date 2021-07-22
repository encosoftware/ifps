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
    [Route("api/workstationtypes")]
    [ApiController]
    public class WorkStationTypeController : IFPSControllerBase
    {
        private const string OPNAME = "WorkStationTypes";
        private readonly IWorkStationTypesAppService workStationTypesAppService;

        public WorkStationTypeController(IWorkStationTypesAppService workStationTypesAppService)
        {
            this.workStationTypesAppService = workStationTypesAppService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<WorkStationTypeListDto>> GetWorkStationTypes()
        {
            return workStationTypesAppService.GetWorkStationTypesAsync();
        }

        [HttpGet("machines")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<WorkStationTypeListWithMachinesDto>> GetWorkStationTypesWithMachines()
        {
            return workStationTypesAppService.GetWorkStationTypesWithMachinesAsync();
        }
    }
}
