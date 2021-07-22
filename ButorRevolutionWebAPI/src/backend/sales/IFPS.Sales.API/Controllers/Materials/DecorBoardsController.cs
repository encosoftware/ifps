using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/decorboards")]
    [ApiController]
    public class DecorBoardsController : IFPSControllerBase
    {
        private const string OPNAME = "DecorBoards";

        private readonly IDecorBoardMaterialAppService decorBoardMaterialAppService;

        public DecorBoardsController(
            IDecorBoardMaterialAppService decorBoardMaterialAppService)
        {
            this.decorBoardMaterialAppService = decorBoardMaterialAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<DecorBoardMaterialListDto>> GetAccessoryMaterials([FromQuery]DecorBoardMaterialFilterDto filter)
        {
            return decorBoardMaterialAppService.GetDecorBoardMaterialsAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateDecorBoardMaterial([FromBody] DecorBoardMaterialCreateDto model)
        {
            return decorBoardMaterialAppService.CreateDecorBoardMaterialAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PutDecorBoardMaterial(Guid id, [FromBody] DecorBoardMaterialUpdateDto model)
        {
            return decorBoardMaterialAppService.UpdateDecorBoardMaterialAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<DecorBoardMaterialDetailsDto> GetDecorBoardMaterialById(Guid id)
        {
            return decorBoardMaterialAppService.GetDecorBoardMaterialAsync(id);
        }

        [HttpGet("codes")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DecorBoardMaterialCodesDto>> GetDecorBoardMaterialCodes()
        {
            return decorBoardMaterialAppService.GetDecorBoardMaterialCodesAsync();
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DecorBoardMaterialWithImageDto>> GetDecorBoardMaterialsForDropdown()
        {
            return decorBoardMaterialAppService.GetDecorBoardMaterialForDropdownAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteDecorBoardMaterial(Guid id)
        {
            return decorBoardMaterialAppService.DeleteMaterialAsync(id);
        }
    }
}