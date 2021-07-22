using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IWebshopOrdersAppService
    {
        Task<List<WebshopOrdersListDto>> GetWebshopOrdersByCustomerIdByWebshopAsync(int customerId);
        Task<WebshopOrdersDetailsDto> GetOrderedFurnitureUnitsDetailsAsync(Guid webshopOrderId);
    }
}
