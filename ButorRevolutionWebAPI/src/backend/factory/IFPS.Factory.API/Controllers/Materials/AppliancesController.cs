using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/appliances")]
    [ApiController]
    public class AppliancesController : IFPSControllerBase
    {
        private const string OPNAME = "Appliances";

        private readonly IApplianceMaterialAppService applianceMaterialAppService;

        public AppliancesController(
            IApplianceMaterialAppService applianceMaterialAppService)
        {
            this.applianceMaterialAppService = applianceMaterialAppService;
        }

        [HttpPost("fromfile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateApplianceFromFile(ApplianceMaterialCreateFromGeneratedDataDto dto)
        {
            return applianceMaterialAppService.CreateApplianceFromFileAsync(dto);
        }

        [HttpGet("datageneration")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<ApplianceMaterialLisForDataGenerationDto>> GetApplianceMaterials()
        {
            return applianceMaterialAppService.GetApplianceMaterialsAsync();
        }
    }
}