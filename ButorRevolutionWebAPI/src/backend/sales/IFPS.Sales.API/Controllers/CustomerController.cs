using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : IFPSControllerBase
    {
        private const string OPNAME = "Customers";
        private readonly ICustomerAppService customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            this.customerAppService = customerAppService;
        }

        //POST: api/customers
        [HttpPut("customers/{customerId}, {dto}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateProductRecommendations(int customerId, FurnitureRecommendationDto dto)
        {
            return customerAppService.UpdateProductRecommendationsAsync(customerId, dto);
        }

        //GET recommended products for customers or trending products for non customers
        [HttpGet("{customerId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetFurnitureRecommendationsForCustomer(int customerId)
        {
            return customerAppService.GetProductRecommendationsAsync(customerId);
        }
    }
}
