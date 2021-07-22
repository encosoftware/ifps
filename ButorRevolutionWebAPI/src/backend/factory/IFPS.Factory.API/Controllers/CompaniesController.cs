using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    //[Authorize]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : IFPSControllerBase
    {
        private const string OPNAME = "Companies";

        private readonly ICompanyAppService companyAppService;

        public CompaniesController(
            ICompanyAppService companyAppService)
        {
            this.companyAppService = companyAppService;
        }

        // GET company list
        [HttpGet]
        [Authorize(Policy = "GetCompanies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CompanyListDto>> Get([FromQuery]CompanyFilterDto filter)
        {
            return companyAppService.GetCompaniesAsync(filter);
        }

        //GET company by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetCompanies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CompanyDetailsDto> GetById(int id)
        {
            return companyAppService.GetCompanyDetailsAsync(id);
        }

        // POST: api/companies
        [HttpPost]
        [Authorize(Policy = "UpdateCompanies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateCompany([FromBody] CompanyCreateDto createDto)
        {
            return companyAppService.CreateCompanyAsync(createDto);
        }

        // PUT: api/companies/
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateCompanies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateCompany(int id, [FromBody]CompanyUpdateDto updateDto)
        {
            return companyAppService.UpdateCompanyAsync(id, updateDto);
        }

        // DELETE: api/companies/
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteCompanies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteCompany(int id)
        {
            return companyAppService.DeleteCompanyAsync(id);
        }

        // Supplier list for dropdown
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<SupplierDropdownListDto>> SupplierDropdownList()
        {
            return companyAppService.GetSuppliersForDropdownAsync();
        }
    }
}