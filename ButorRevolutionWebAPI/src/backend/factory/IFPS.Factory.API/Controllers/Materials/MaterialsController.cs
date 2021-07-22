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
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : IFPSControllerBase
    {
        private const string OPNAME = "Materials";

        private readonly IMaterialAppService materialAppService;

        public MaterialsController(
            IMaterialAppService materialAppService)
        {
            this.materialAppService = materialAppService;
        }

        [HttpGet("dropdown/{categoryId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdownByCategory(int categoryId)
        {
            return materialAppService.GetMaterialsForDropdownByCategoryAsync(categoryId);
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdown()
        {
            return materialAppService.GetMaterialsForDropdownAsync();
        }
    }
}