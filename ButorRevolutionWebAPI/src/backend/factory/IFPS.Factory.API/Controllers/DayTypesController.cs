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
    [Route("api/daytypes")]
    [ApiController]
    public class DayTypesController : IFPSControllerBase
    {
        private const string OPNAME = "DayTypes";

        private readonly IDayTypeAppService dayTypeAppService;

        public DayTypesController(
            IDayTypeAppService dayTypeAppService)
        {
            this.dayTypeAppService = dayTypeAppService;
        }

        // GET daytype list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<DayTypeListDto>> GetDayTypes()
        {
            return await dayTypeAppService.GetDayTypesAsync();
        }

        // GET weekdays daytype list
        [HttpGet("weekdays")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<DayTypeListDto>> GetWeekDays()
        {
            return await dayTypeAppService.GetWeekDaysAsync();
        }
    }
}