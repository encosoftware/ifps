using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/machines")]
    [ApiController]
    public class MachinesController : IFPSControllerBase
    {
        private const string OPNAME = "Machines";
        private readonly IMachineAppService machineAppService;

        public MachinesController(IMachineAppService machineAppService)
        {
            this.machineAppService = machineAppService;
        }       

        [HttpGet("dropdown/machines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MachinesDropdownDto>> GetMachinesDropdown()
        {
            return machineAppService.GetMachinesDropdownAsync();
        }
    }
}
