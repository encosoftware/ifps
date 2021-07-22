using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : IFPSControllerBase
    {
        private const string OPNAME = "Countries";

        private readonly ICountryAppService countryAppService;

        public CountriesController(
           ICountryAppService countryAppService)
        {
            this.countryAppService = countryAppService;
        }

        // GET country list
        [HttpGet]
       // [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<CountryListDto>> GetCountries()
        {
            return await countryAppService.GetCountriesAsync();
        }
    }
}