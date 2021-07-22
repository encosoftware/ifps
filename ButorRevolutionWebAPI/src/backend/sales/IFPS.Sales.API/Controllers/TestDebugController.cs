using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IFPS.Sales.API.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Sales.API.Controllers
{

    [Route("api/testdebug")]
    [ApiController]
    public class TestDebugController : IFPSControllerBase
    {
        private const string OPNAME = "TestDebug";

        private readonly IConfiguration configuration;
        public TestDebugController(
           IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("connectionString")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public string GetConnString()
        {
            return configuration.GetSection("ConnectionStrings")["DefaultConnection"];
        }


        [HttpGet("baseUrl")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public string GetBaseUrl()
        {
            return configuration.GetSection("Site")["BaseUrl"];
        }

        
        [HttpGet("cors")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public string Getcors()
        {
            return configuration["AllowedHosts"];
        }

        [HttpGet("culture")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public string GetCulture()
        {
            return CultureInfo.CurrentCulture.Name;
        }


    }
}