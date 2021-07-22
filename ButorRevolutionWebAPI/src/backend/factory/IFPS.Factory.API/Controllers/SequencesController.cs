using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/sequences")]
    [ApiController]
    public class SequencesController : IFPSControllerBase
    {
        private const string OPNAME = "Sequences";
        private readonly ISequenceAppService sequenceAppService;

        public SequencesController(ISequenceAppService sequenceAppService)
        {
            this.sequenceAppService = sequenceAppService;
        }

        [HttpPost("rectangle")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<int> CreateRectangelBySequence(Guid componentId, RectangleBySequenceCreateDto dto)
        {
            return await sequenceAppService.CreateRectangleBySequenceAsync(componentId, dto);
        }

        [HttpPost("drill")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<int> CreateDrillBySequence(Guid componentId, DrillBySequenceCreateDto dto)
        {
            return await sequenceAppService.CreateDrillBySequenceAsync(componentId, dto);
        }
    }
}
