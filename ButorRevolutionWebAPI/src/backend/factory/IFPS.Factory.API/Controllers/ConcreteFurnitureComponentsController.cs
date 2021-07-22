using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/concretefurniturecomponents")]
    [ApiController]
    public class ConcreteFurnitureComponentsController : IFPSControllerBase
    {
        private const string OPNAME = "ConcreteFurnitureComponents";

        private readonly IConcreteFurnitureComponentAppService concreteFurnitureComponentAppService;

        public ConcreteFurnitureComponentsController(IConcreteFurnitureComponentAppService concreteFurnitureComponentAppService)
        {
            this.concreteFurnitureComponentAppService = concreteFurnitureComponentAppService;
        }

        [HttpGet("{orderId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<ConcreteFurnitureComponentInformationListDto>> GetCFCsForPrintByOrderId(Guid orderId)
        {
            return await concreteFurnitureComponentAppService.GetConcreteFurnitureComponentsAsync(orderId);
        }
    }
}