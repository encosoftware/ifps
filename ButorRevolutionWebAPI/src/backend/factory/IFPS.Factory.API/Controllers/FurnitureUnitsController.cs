using ENCO.DDD.Application.Dto;
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
    [Route("api/furnitureunits")]
    [ApiController]
    public class FurnitureUnitsController : IFPSControllerBase
    {
        private const string OPNAME = "FurnitureUnits";

        private readonly IFurnitureUnitAppService furnitureUnitAppService;

        public FurnitureUnitsController(IFurnitureUnitAppService furnitureUnitAppService)
        {
            this.furnitureUnitAppService = furnitureUnitAppService;
        }

        // CREATE furniture unit from .cvs file
        [HttpPost("file")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PostFurnitureUnitFromFile(FurnitureUnitDetailsFromFileDto dto, string containerName, string fileName)
        {
            return furnitureUnitAppService.CreateFurnitureUnitFromFileAsync(dto, containerName, fileName);
        }

        // GET all furniture units for data generation
        [HttpGet("datageneration")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitForDataGenerationListDto>> GetFurnitureUnitsForDataGeneration()
        {
            return furnitureUnitAppService.GetFurnitureUnitsForDataGenerationAsync();
        }

        //Get all furniture units
        [HttpGet]
        [Authorize("GetFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnits([FromQuery] FurnitureUnitFilterDto dto)
        {
            return furnitureUnitAppService.GetFurnitureUnitsAsync(dto);
        }

        [HttpPost("xxl")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UploadXxlFileForCncGeneration(Guid furnitureUnitId, string containerName, string fileName)
        {
            return furnitureUnitAppService.ParseXxlDataForCncGeneration(furnitureUnitId, containerName, fileName);
        }
    }
}