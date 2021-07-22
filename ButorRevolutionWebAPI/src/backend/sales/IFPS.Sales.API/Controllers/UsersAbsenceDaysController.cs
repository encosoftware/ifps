using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/users/{userId}/absencedays")]
    [ApiController]
    public class UsersAbsenceDaysController : IFPSControllerBase
    {
        private const string OPNAME = "UsersAbsenceDays";
        private readonly IAbsenceDaysAppService absenceDaysAppService;

        public UsersAbsenceDaysController(IAbsenceDaysAppService absenceDaysAppService)
        {
            this.absenceDaysAppService = absenceDaysAppService;
        }

        // GET AbsenceDays list
        [HttpGet]
        [Authorize(Policy = "GetUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AbsenceDayDto>> GetAbsenceDays(int userId, [FromQuery]int year, [FromQuery]int month)
        {
            return absenceDaysAppService.GetAbsenceDaysAsync(userId, year, month);
        }

        [HttpPut]
        [Authorize(Policy = "UpdateUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddOrUpdateAbsenceDays(int userId, [FromBody]List<AbsenceDayDto> absenceDays)
        {
            return absenceDaysAppService.AddOrUpdateAbsenceDaysAsync(userId, absenceDays);
        }

        [HttpDelete]
        [Authorize(Policy = "DeleteUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteAbsenceDays(int userId, [FromBody] AbsenceDaysDeleteDto absenceDays)
        {
            return absenceDaysAppService.DeleteAbsenceDaysAsync(userId, absenceDays);
        }
    }
}