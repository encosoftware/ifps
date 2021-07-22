using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/optimization")]
    [ApiController]
    public class OptimizationController : IFPSControllerBase
    {
        private const string OPNAME = "Optimization";

        private readonly IOptimizationAppService optimizationAppService;

        public OptimizationController(IOptimizationAppService optimizationAppService)
        {
            this.optimizationAppService = optimizationAppService;
        }

        // GET optimizations list
        [HttpGet]
        [Authorize(Policy = "GetOptimizations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OptimizationListDto>> GetOptimizations([FromQuery] OptimizationFilterDto optimizationFilterDto)
        {
            return optimizationAppService.GetOptimizationsAsync(optimizationFilterDto);
        }

        // POST start optimization of given orders
        [HttpPost]
        //[Authorize(Policy = "UpdateOptimizations")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task StartOrdersOptimization([FromBody] OrdersForOptimizationDto ordersDto)
        {
            return optimizationAppService.StartOrdersOptimization(ordersDto, GetCallerId());
        }

        // GET layout plans as zip
        [HttpGet("layout/zip/{id}")]
        //[Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileContentResult> DownloadLayoutAsZip(Guid id)
        {
            var zipByteArray = await optimizationAppService.GetLayoutZipAsBytesAsync(id);
            return new FileContentResult(zipByteArray, "application/zip");
        }

        // GET schedule html as zip
        [HttpGet("schedule/zip/{id}")]
        //[Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileContentResult> DownloadScheduleAsZip(Guid id)
        {
            var zipByteArray = await optimizationAppService.GetScheduleZipAsBytesAsync(id);
            return new FileContentResult(zipByteArray, "application/zip");
        }
    }
}