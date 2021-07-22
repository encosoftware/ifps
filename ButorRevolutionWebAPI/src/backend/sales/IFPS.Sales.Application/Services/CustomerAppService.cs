using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private const int SMALLEST_CUSTOMER_ID = 1;
        private readonly ICustomerRepository customerRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository;

        public CustomerAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICustomerRepository customerRepository,
            IFurnitureUnitRepository furnitureUnitRepository,
            IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository) : base(aggregate)
        {
            this.customerRepository = customerRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.webshopFurnitureUnitRepository = webshopFurnitureUnitRepository;
        }

        public async Task UpdateProductRecommendationsAsync(int customerId, FurnitureRecommendationDto dto)
        {
            var customer = await customerRepository.SingleIncludingAsync(x => x.Id == customerId, x => x.RecommendedProducts);
            await AddRecommendedFurnitureToCustomer(dto, customer);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetProductRecommendationsAsync(int customerId)
        {
            if (customerId >= SMALLEST_CUSTOMER_ID)
            {
                return await GetRecommendedProductsByCustomerIdAsync(customerId);
            }
            else
            {
                List<Guid> ids = await GetTrendingProductsAsync();
                var wfus = await webshopFurnitureUnitRepository
                    .GetAllListIncludingAsync(ent => ids.Contains(ent.FurnitureUnitId), ent => ent.Price.Currency, ent => ent.FurnitureUnit.Image, ent => ent.Images);

                var result = wfus.Select(x => new WebshopFurnitureUnitListByWebshopCategoryDto(x)).ToList();
                return result;
            }
        }

        #region Private methods

        private async Task AddRecommendedFurnitureToCustomer(FurnitureRecommendationDto dto, Customer customer)
        {
            CustomerFurnitureUnit unit;
            customer.ClearRecommendations();

            foreach (Guid furnitureUnitId in dto.Ids)
            {
                var wfu = await webshopFurnitureUnitRepository.FirstOrDefaultAsync(ent => ent.FurnitureUnitId == furnitureUnitId);
                unit = new CustomerFurnitureUnit(customer.Id, wfu.Id);
                customer.AddRecommendation(unit);
            }
        }

        private async Task<List<Guid>> GetTrendingProductsAsync()
        {
            var furnitureUnits = await furnitureUnitRepository.GetTrendingProductsAsync();
            return furnitureUnits.Select(units => units.Id).ToList();
        }

        private async Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetRecommendedProductsByCustomerIdAsync(int customerId)
        {
            var result = await customerRepository.GetRecommendedProductsByCustomerId(customerId);
            return result.RecommendedProducts.Select(x => new WebshopFurnitureUnitListByWebshopCategoryDto(x.WebshopFurnitureUnit)).ToList();
        }
        #endregion
    }
}
