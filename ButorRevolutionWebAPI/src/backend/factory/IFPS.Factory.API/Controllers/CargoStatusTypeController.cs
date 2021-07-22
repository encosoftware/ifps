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
    [Route("api/cargostatustypes")]
    [ApiController]
    public class CargoStatusTypeController : IFPSControllerBase
    {
        private const string OPNAME = "CargoStatusTypes";

        private readonly ICargoStatusTypeAppService service;

        public CargoStatusTypeController(ICargoStatusTypeAppService service)
        {
            this.service = service;
        }

        // CargoStatusType list for dropdown
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<CargoStatusTypeDropdownListDto>> CargoStatusTypeDropdownList()
        {
            return service.GetCargoStatusTypeForDropdownAsync();
        }
    }
}
