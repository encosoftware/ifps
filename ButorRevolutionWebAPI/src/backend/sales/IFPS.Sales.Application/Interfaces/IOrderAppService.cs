using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using IFPS.Sales.Domain.Enums;
using System;
using IFPS.Sales.Application.Dto.Users;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IOrderAppService
    {
        Task<PagedListDto<OrderListDto>> GetOrdersAsync(OrderFilterDto filterDto, int? userId, DivisionTypeEnum? division);
        Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompanyAsync(int companyId, OrderFinanceFilterDto filter);
        Task<Guid> CreateOrderAsync(OrderCreateDto orderDto);
        Task DeleteOrderAsync(Guid id);
        Task<OrderSalesHeaderDto> GetOrderDetailsAsync(Guid id, int userId);
        Task CreateOfferAsync(OfferCreateDto dto, Guid orderId);
        Task<OfferDetailsDto> GetFurnitureUnitsListByOfferAsync(Guid orderId);
        Task AddOrderedFurnitureUnitAsync(Guid orderId, FurnitureUnitCreateByOfferDto dto);
        Task<FurnitureUnitDetailsByOfferDto> GetOrderedFurnitureUnitAsync(Guid orderId, int orderedFurnitureUnitId);
        Task UpdateOrderedFurnitureUnitQuantityAsync(Guid orderId, int orderedFurnitureId, UpdateOrderedFurnitureUnitQuantityByOfferDto dto);
        Task<Guid> CreateFurnitureUnitByOfferAsync(Guid orderId, Guid baseFurnitureUnitId, FurnitureUnitCreateWithQuantityByOfferDto dto);
        Task DeleteOrderedFurnitureUnitAsync(Guid orderId, Guid furnitureUnitId);
        Task AddApplianceAsync(Guid orderId, ApplianceCreateByOfferDto dto);
        Task<ApplianceDetailsByOfferDto> GetApplianceAsync(Guid orderId, int orderedApplianceMaterialId);
        Task UpdateApplianceAsync(Guid orderId, int orderedApplianceMaterialId, ApplianceUpdateByOfferDto dto);
        Task DeleteApplianceFromAplliancesListAsync(Guid orderId, int orderedApplianceMaterialId);
        Task<OfferPreviewDto> OfferPreview(Guid orderId);
        Task AddServiceAsync(Guid orderId, ServiceCreateByOfferDto dto);
        Task SetVatAsync(Guid orderId, bool isVat);
        Task UploadDocuments(Guid id, DocumentUploadDto documentDto);
        Task ApproveDocuments(Guid orderId, int documentGroupVersionId, DocumentStateEnum result, int callerId);
        Task UpdateOrderState(Guid id, OrderEditDto updateDto);
        Task AddOrderPaymentAsync(Guid id, OrderFinanceCreateDto orderFinanceCreateDto);
        Task<List<DocumentGroupDto>> GetDocumentsOfOrder(Guid orderId);
        Task<ContractDetailsDto> GetContractAsync(Guid orderId);
        Task CreateContractAsync(Guid orderId, ContractCreateDto dto);
        
        Task<UserDropdownAvatarDto> GetCustomerByOrderAsync(Guid orderId);
        Task SetShippingStateAsync(Guid orderId);
        Task SetInstallationStateAsync(Guid orderId);
        Task SetWaitingForContractStateAsync(Guid orderId, int partnerId);
        Task DownloadDocuments(Guid orderId, System.IO.MemoryStream memoryStream);
    }
}
