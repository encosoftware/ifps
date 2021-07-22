using System.Collections.Generic;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/materialpackages")]
    [ApiController]
    public class MaterialPackageController : IFPSControllerBase
    {
        private const string OPNAME = "MaterialPackages";

        private readonly IMaterialPackageAppService materialPackageAppService;

        public MaterialPackageController(
           IMaterialPackageAppService materialPackageAppService)
        {
            this.materialPackageAppService = materialPackageAppService;
        }

        // GET package code list
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<MaterialPackageCodeListDto>> GetPackageCodes()
        {
            return await materialPackageAppService.GetPackageCodes();
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterialPackages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<MaterialPackageListDto>> GetMaterialPackages([FromQuery] MaterialPackageFilterDto filter)
        {
            return materialPackageAppService.GetMaterialPackagesAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterialPackages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateMaterialPackage([FromBody] MaterialPackageCreateDto model)
        {
            return materialPackageAppService.CreatMaterialPackageAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterialPackages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateMaterialPackage(int id, [FromBody] MaterialPackageUpdateDto model)
        {
            return materialPackageAppService.UpdateMaterialPackageAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterialPackages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<MaterialPackageDetailsDto> GetMaterialPackageDetails(int id)
        {
            return materialPackageAppService.GeMaterialPackageAsync(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterialPackages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteMaterialPackage(int id)
        {
            return materialPackageAppService.DeleteMaterialPackageAsync(id);
        }

        [HttpPost("import")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ImportMaterialPackagesFromCsv(MaterialPackageImportDto materialPackageImportDto)
        {
            return materialPackageAppService.ImportMaterialPackagesFromCsv(materialPackageImportDto);
        }
    }
}