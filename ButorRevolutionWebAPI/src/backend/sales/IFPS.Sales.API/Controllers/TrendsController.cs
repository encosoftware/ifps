using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/trends")]
    [ApiController]
    public class TrendsController : IFPSControllerBase
    {
        private const string OPNAME = "Trends";

        private readonly ITrendsAppService trendsAppService;

        public TrendsController(ITrendsAppService trendsAppService)
        {
            this.trendsAppService = trendsAppService;
        }

        [HttpGet("furnitureunits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<TrendListDto<FurnitureUnitPreviewDto>> GetFurnitureUnitOrdersTrend(int Take = 10, [FromQuery]DateTime? IntervalFrom = null, [FromQuery] DateTime? IntervalTo = null)
        {
            return trendsAppService.ListFurnitureUnitOrdersTrend(Take, IntervalFrom, IntervalTo);
        }

        [HttpGet("boardmaterials/front")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<TrendListDto<BoardMaterialPreviewDto>> GetFrontFurnitureComponentsOrdersTrend(int Take = 10, [FromQuery]DateTime? IntervalFrom = null, [FromQuery] DateTime? IntervalTo = null)
        {
            return trendsAppService.ListBoardMaterialOrdersTrend(FurnitureComponentTypeEnum.Front, Take, IntervalFrom, IntervalTo);
        }

        [HttpGet("boardmaterials/corpus")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<TrendListDto<BoardMaterialPreviewDto>> GetCorpusFurnitureComponentsOrdersTrend(int Take = 10, [FromQuery]DateTime? IntervalFrom = null, [FromQuery] DateTime? IntervalTo = null)
        {
            return trendsAppService.ListBoardMaterialOrdersTrend(FurnitureComponentTypeEnum.Corpus, Take, IntervalFrom, IntervalTo);
        }

    }
}
