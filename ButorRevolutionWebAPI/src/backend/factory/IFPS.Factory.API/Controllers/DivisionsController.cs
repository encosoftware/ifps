using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/divisions")]
    [ApiController]
    public class DivisionsController : IFPSControllerBase
    {
        private const string OPNAME = "Divisions";

        private readonly IDivionAppService divionAppService;

        public DivisionsController(IDivionAppService divionAppService)
        {
            this.divionAppService = divionAppService;
        }

        // GET Claims list
        [HttpGet("claims")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DivisionClaimsListDto>> GetDivisionClaimsList()
        {
            return divionAppService.GetDivisionClaimsListAsync();
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DivisionListDto>> GetDivisionList()
        {
            return divionAppService.GetDivisionListAsync();
        }
    }
}