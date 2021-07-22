using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
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

        [HttpPost("fromfile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task CreateBoardsFromFile(string container, string fileName)
        {
            return decorBoardMaterialAppService.CreateBoardsFromFileAsync(container, fileName);
        }
    }
}