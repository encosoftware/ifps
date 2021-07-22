﻿using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/companytypes")]
    [ApiController]
    public class CompanyTypesController : IFPSControllerBase
    {
        private const string OPNAME = "CompanyTypes";

        private readonly ICompanyTypeAppService companyTypeAppService;

        public CompanyTypesController(
            ICompanyTypeAppService companyTypeAppService)
        {
            this.companyTypeAppService = companyTypeAppService;
        }

        // GET companytype list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<CompanyTypeListDto>> GetCompanyTypes()
        {
            return await companyTypeAppService.GetCompanyTypesAsync();
        }
    }
}