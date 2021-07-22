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
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : IFPSControllerBase
    {
        private const string OPNAME = "Languages";

        private readonly ILanguageAppService languageAppService;

        public LanguageController(ILanguageAppService languageAppService)
        {
            this.languageAppService = languageAppService;
        }

        // GET Language list with translations
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<LanguageListDto>> Get()
        {
            return languageAppService.GetLanguages();
        }
    }
}