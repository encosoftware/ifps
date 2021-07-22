using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers.Materials
{
    [Route("api/boards")]
    [ApiController]
    public class BoardMaterialController : IFPSControllerBase
    {
        private const string OPNAME = "Boards";
        private readonly IBoardMaterialAppService service;

        public BoardMaterialController(IBoardMaterialAppService service)
        {
            this.service = service;
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<BoardMaterialsForDropdownDto>> GetBoardMaterials()
        {
            return service.GetBoardMaterialsAsync();
        }
    }
}
