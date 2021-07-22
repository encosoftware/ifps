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
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : IFPSControllerBase
    {
        private const string OPNAME = "Currencies";

        private readonly ICurrencyAppService currencyAppService;

        public CurrencyController(ICurrencyAppService currencyAppService)
        {
            this.currencyAppService = currencyAppService;
        }

        // GET currencies list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<CurrencyDto>> GetCurrencies()
        {
            return currencyAppService.GetCurrenciesListAsync();
        }
    }
}