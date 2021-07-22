using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/furniturecomponents")]
    [ApiController]
    public class FurnitureComponentsController : IFPSControllerBase
    {
        private const string OPNAME = "FurnitureComponents";

        private readonly IFurnitureComponentAppService furnitureComponentAppService;

        public FurnitureComponentsController(
            IFurnitureComponentAppService furnitureComponentAppService)
        {
            this.furnitureComponentAppService = furnitureComponentAppService;
        }

        [HttpGet("{id}/sequence")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FurnitureComponentWithSequenceDetailsDto> GetComponentWithSequence(Guid id)
        {
            return furnitureComponentAppService.GetComponentWithSequenceAsync(id);
        }

        // GET components by unit
        [HttpGet("{furnitureUnitId}/datageneration/components")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitWithComponentsDto>> GetComponentsByUnitId(Guid furnitureUnitId)
        {
            return furnitureComponentAppService.GetComponentsByUnitIdAsync(furnitureUnitId);
        }
    }
}