using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/worktopboards")]
    [ApiController]
    public class WorktopBoardsController : IFPSControllerBase
    {
        private const string OPNAME = "WorktopBoards";

        private readonly IWorktopBoardMaterialAppService worktopBoardMaterialAppService;

        public WorktopBoardsController(
            IWorktopBoardMaterialAppService worktopBoardMaterialAppService)
        {
            this.worktopBoardMaterialAppService = worktopBoardMaterialAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<WorktopBoardMaterialListDto>> GetAccessoryMaterials([FromQuery]WorktopBoardMaterialFilterDto filter)
        {
            return worktopBoardMaterialAppService.GetWorktopBoardMaterialsAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateWorktopBoardMaterial([FromBody] WorktopBoardMaterialCreateDto model)
        {
            return worktopBoardMaterialAppService.CreateWorktopBoardMaterialAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PutWorktopBoardMaterial(Guid id, [FromBody] WorktopBoardMaterialUpdateDto model)
        {
            return worktopBoardMaterialAppService.UpdateWorktopBoardMaterialAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WorktopBoardMaterialDetailsDto> GetWorktopBoardMaterialById(Guid id)
        {
            return worktopBoardMaterialAppService.GetWorktopBoardMaterialAsync(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteWorktopBoardMaterial(Guid id)
        {
            return worktopBoardMaterialAppService.DeleteMaterialAsync(id);
        }
    }
}