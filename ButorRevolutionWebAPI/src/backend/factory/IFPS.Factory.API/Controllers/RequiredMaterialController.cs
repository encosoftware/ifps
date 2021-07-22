using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/requiredmaterials")]
    [ApiController]
    public class RequiredMaterialController : IFPSControllerBase
    {
        private const string OPNAME = "RequiredMaterials";

        private readonly IRequiredMaterialsAppService service;

        public RequiredMaterialController(IRequiredMaterialsAppService service)
        {
            this.service = service;
        }
        
        // Get all requiredMaterials
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<RequiredMaterialsListDto>> List([FromQuery] RequiredMaterialsFilterDto filterDto)
        {
            return service.GetRequiredMaterialsListAsync(filterDto);
        }

        // Create temp cargo for materials and additionals list
        [HttpPost("selected")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<TempCargoDetailsForRequiredMaterialsDto> CreateSelectedRequiredMaterial([FromBody] SelectedRequiredMaterialsDto dto)
        {
            return service.CreateSelectedRequiredMaterials(dto);
        }

        // Get all material code (for dropdwon list)
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MaterialCodeListDto>> MaterialCodeList()
        {
            return service.GetMaterialCodesAsync();
        }

        // POST create required material
        [HttpPost("{orderId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateRequiredMaterialByOrderId(Guid orderId)
        {
            return service.CreateRequiredMaterialByOrderIdAsync(orderId, GetCallerId());
        }

        // GET export csv file
        [HttpGet("export")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> ExportCsv([FromQuery]RequiredMaterialsFilterDto filterDto)
        {
            var stream = new MemoryStream();
            var csv = await service.ExportCsvAsync(stream, filterDto);
            return File(stream, "application/octet-stream", csv);
        }
    }
}
