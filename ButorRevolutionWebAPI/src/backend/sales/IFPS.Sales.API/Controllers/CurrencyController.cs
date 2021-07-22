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