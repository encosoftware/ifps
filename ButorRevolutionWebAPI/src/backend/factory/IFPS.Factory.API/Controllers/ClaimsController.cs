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
    [Route("api/claims")]
    [ApiController]
    public class ClaimsController : IFPSControllerBase
    {
        private const string OPNAME = "Claims";

        private readonly IClaimAppService claimAppService;

        public ClaimsController(IClaimAppService claimAppService)
        {
            this.claimAppService = claimAppService;
        }

        // GET claims with group list
        [HttpGet("groups")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<ClaimGroupDto>> GetClaimsWithGroupsList()
        {
            return claimAppService.GetClaimsWithGroupsListAsync();
        }

        // GET Claims list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<string>> GetClaimsList()
        {
            return claimAppService.GetClaimsListAsync();
        }
    }
}