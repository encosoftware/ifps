using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Application.Interfaces.Material;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/dataimport")]
    [ApiController]
    public class DataImportController : IFPSControllerBase
    {
        private const string OPNAME = "DataImport";
        private readonly IDataImportAppService service;

        public DataImportController(
            IDataImportAppService dataImportAppService
        )
        {
            this.service = dataImportAppService;
        }

        [HttpPost("importmaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ImportMaterialsFromCsv(MaterialImportDto materialImportDto)
        {
            return service.ImportMaterialsFromCsv(materialImportDto);
        }
    }
}
