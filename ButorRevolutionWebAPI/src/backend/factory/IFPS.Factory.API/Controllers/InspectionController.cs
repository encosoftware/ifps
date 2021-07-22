using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/inspections")]
    [ApiController]
    public class InspectionController : IFPSControllerBase
    {
        private const string OPNAME = "Inspections";
        private readonly IInspectionAppService service;

        public InspectionController(IInspectionAppService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateInspectionAsync([FromBody]InspectionCreateDto inspectionCreateDto)
        {
            return service.CreateInspectionAsync(inspectionCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<InspectionListDto>> GetInspectionsAsync([FromQuery]InspectionFilterDto inspectionFilterDto)
        {
            return service.GetInspectionsAsync(inspectionFilterDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<InspectionDetailsDto> GetInspectionAsync(int id)
        {
            return service.GetInspectionAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateInspectionAsync(int id, [FromBody] InspectionUpdateDto inspectionUpdateDto)
        {
            return service.UpdateInspectionAsync(id, inspectionUpdateDto);
        }


        [HttpGet("reports/{id}")]
        [Authorize(Policy = "GetInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<ReportDetailsDto> GetInspectionReportAsync(int id)
        {
            return service.GetInspectionReportAsync(id);
        }

        [HttpPut("reports/{id}")]
        [Authorize(Policy = "UpdateInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateInspectionReportAsync(int id, [FromBody] ReportUpdateDto reportUpdateDto)
        {
            return service.UpdateInspectionReportAsync(id, reportUpdateDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteInspectionAsync(int id)
        {
            return service.DeleteInspectionAsync(id);
        }

        [HttpPut("reports/close/{id}")]
        [Authorize(Policy = "UpdateInspections")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task CloseInspectionReportAsync(int id)
        {
            return service.CloseInspectionReportAsync(id);
        }
    }
}
