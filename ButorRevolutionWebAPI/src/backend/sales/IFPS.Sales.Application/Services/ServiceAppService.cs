using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class ServiceAppService : ApplicationService, IServiceAppService
    {
        private readonly IServiceRepository serviceRepository;

        public ServiceAppService(
            IServiceRepository serviceRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.serviceRepository = serviceRepository;
        }

        public async Task<List<ServiceListDto>> GetServicesForDropdownAsync()
        {
            var services = await serviceRepository.GetAllListIncludingAsync(ent => true, ent => ent.CurrentPrice.Price.Currency, ent => ent.ServiceType);
            return services.Select(ent => new ServiceListDto(ent)).ToList();
        }

        public async Task<List<ShippingServiceListDto>> GetShippingServicesForDropdownAsync()
        {
            var services = await serviceRepository.GetAllListIncludingAsync(ent => ent.ServiceType.Type == ServiceTypeEnum.Shipping, ent => ent.CurrentPrice.Price.Currency);
            return services.Select(ent => new ShippingServiceListDto(ent)).ToList();
        }
    }
}
