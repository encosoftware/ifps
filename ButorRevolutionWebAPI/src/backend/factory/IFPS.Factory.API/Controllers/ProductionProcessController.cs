using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [ApiController]
    [Route("api/productionprocess")]
    public class ProductionProcessController : IFPSControllerBase
    {
        private const string OPNAME = "ProductionProcess";
        private readonly IProductionProcessAppService productionProcessAppService;

        public ProductionProcessController(IProductionProcessAppService productionProcessAppService)
        {
            this.productionProcessAppService = productionProcessAppService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateProductionProcess")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetProcessStatus(int id)
        {
            return productionProcessAppService.SetProcessStatusAsync(id, GetCallerId());
        }
    }
}
