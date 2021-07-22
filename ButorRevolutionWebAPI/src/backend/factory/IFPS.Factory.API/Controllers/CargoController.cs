using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Dto.Cargos;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/cargos")]
    [ApiController]
    public class CargoController : IFPSControllerBase
    {
        private const string OPNAME = "Cargos";
        private readonly ICargoAppService service;

        public CargoController(ICargoAppService service)
        {
            this.service = service;
        }

        // Create new cargo (save cargo)
        [HttpPost]
        [Authorize(Policy = "UpdateCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateCargoFromRequiredMaterials([FromBody] CargoCreateDto dto)
        {
            return service.CreateCargoFromRequiredMaterialsAsync(dto);
        }

        // Get cargo details for Preview
        [HttpGet("{cargoId}/preview")]
        [Authorize(Policy = "GetRequiredMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CargoDetailsPreviewDto> PreviewCargoDetails(int cargoId)
        {
            return service.GetCargoDetailsForPreviewAsync(cargoId);
        }

        // Get all cargo
        [HttpGet]
        [Authorize(Policy = "GetCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CargoListDto>> List([FromQuery]CargoFilterDto dto)
        {
            return service.ListCargos(dto);
        }

        // Get cargo details for missing and refused columns
        [HttpGet("{cargoId}")]
        [Authorize(Policy = "GetCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CargoDetailsDto> CargoDetails(int cargoId)
        {
            return service.CargoDetailsAsync(cargoId);
        }

        // Delete cargo
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteCargo(int id)
        {
            return service.DeleteCargoAsync(id);
        }

        // Update cargo with missing and refused values
        [HttpPut("{cargoId}")]
        [Authorize(Policy = "UpdateCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateProductsByCargo(int cargoId, [FromBody] CargoUpdateDto dto)
        {
            return service.UpdateProductsByCargo(cargoId, dto);
        }

        //Cargo details with all information (missing and refused columns)
        [HttpGet("details/{cargoId}")]
        [Authorize(Policy = "GetCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CargoDetailsWithAllInformationDto> CargoDetailsWithAllInformation(int cargoId)
        {
            return service.CargoDetailsWithAllInformationAsync(cargoId);
        }

        // Get all cargo by stock site
        [HttpGet("bystock")]
        [Authorize(Policy = "GetCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CargoListByStockDto>> ListCargosByStock([FromQuery]CargoFilterDto dto)
        {
            return service.ListCargosByStock(dto);
        }

        // Get cargo details by stock
        [HttpGet("bystock/details/{cargoId}")]
        [Authorize(Policy = "GetCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CargoStockDetailsDto> CargoDetailsByStock(int cargoId)
        {
            return service.GetCargoStockDetailsAsync(cargoId);
        }

        // Update cargo by stock with cell
        [HttpPut("bystock/details/{cargoId}")]
        [Authorize(Policy = "UpdateCargos")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateCargoByStock(int cargoId, [FromBody] UpdateCargoStockDto dto)
        {
            return service.UpdateCargoStock(cargoId, dto);
        }

        // GET export csv file
        [HttpGet("export")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> ExportCsv([FromQuery]CargoFilterDto filterDto)
        {
            var stream = new MemoryStream();
            var csv = await service.ExportCsvAsync(stream, filterDto);
            return File(stream, "application/octet-stream", csv);
        }
    }
}
