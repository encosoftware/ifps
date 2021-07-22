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
    [Route("api/services")]
    [ApiController]
    public class ServiceController : IFPSControllerBase
    {
        public const string OPNAME = "Services";
        public readonly IServiceAppService serviceAppService;

        public ServiceController(IServiceAppService serviceAppService)
        {
            this.serviceAppService = serviceAppService;
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<ServiceListDto>> GetServicesForDropdown()
        {
            return serviceAppService.GetServicesForDropdownAsync();
        }

        [HttpGet("shipping")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<ShippingServiceListDto>> GetShippingServicesForDropdown()
        {
            return serviceAppService.GetShippingServicesForDropdownAsync();
        }
    }
}
