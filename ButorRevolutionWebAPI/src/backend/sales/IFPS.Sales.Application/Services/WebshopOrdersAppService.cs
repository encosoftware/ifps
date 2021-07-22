using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class WebshopOrdersAppService : ApplicationService, IWebshopOrdersAppService
    {
        private readonly IWebshopOrderRepository webshopOrderRepository;

        public WebshopOrdersAppService(IApplicationServiceDependencyAggregate aggregate,
            IWebshopOrderRepository webshopOrderRepository) : base(aggregate)
        {
            this.webshopOrderRepository = webshopOrderRepository;
        }

        public async Task<WebshopOrdersDetailsDto> GetOrderedFurnitureUnitsDetailsAsync(Guid webshopOrderId)
        {
            var webshopOrder = await webshopOrderRepository.GetWebshopOrderWithIncludesById(webshopOrderId);
            return new WebshopOrdersDetailsDto(webshopOrder);
        }

        public async Task<List<WebshopOrdersListDto>> GetWebshopOrdersByCustomerIdByWebshopAsync(int customerId)
        {
            var webshopOrdersFromModel = await webshopOrderRepository.GetAllListIncludingAsync(ent => ent.CustomerId == customerId, ent => ent.Basket.SubTotal.Currency, ent => ent.Basket.DelieveryPrice.Currency);
            var webshopOrders = new List<WebshopOrdersListDto>();
            foreach (var webshopOrder in webshopOrdersFromModel)
            {
                webshopOrders.Add(new WebshopOrdersListDto(webshopOrder)
                {
                    SubTotal = new PriceListDto(webshopOrder.Basket.SubTotal),
                    DelieveryPrice = new PriceListDto(webshopOrder.Basket.DelieveryPrice)
                });
            }
            return webshopOrders;
        }
    }
}
