using System.Collections.Generic;
using System.Threading.Tasks;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Factory.API.Controllers
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
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<CountryListDto>> GetCountries()
        {
            return await countryAppService.GetCountriesAsync();
        }
    }
}