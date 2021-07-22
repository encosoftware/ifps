using System.Collections.Generic;
using System.Threading.Tasks;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/frequencytypes")]
    [ApiController]
    public class FrequencyTypeController : IFPSControllerBase
    {
        private const string OPNAME = "FrequencyTypes";

        private readonly IFrequencyTypeAppService frequencyTypeAppService;

        public FrequencyTypeController(
           IFrequencyTypeAppService frequencyTypeAppService)
        {
            this.frequencyTypeAppService = frequencyTypeAppService;
        }

        // GET frequency types list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<FrequencyTypeListDto>> GetFrequencyTypes()
        {
            return await frequencyTypeAppService.GetFrequencyTypesAsync();
        }
    }
}