using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IFurnitureUnitAppService
    {
        Task<Guid> CreateFurnitureUnitAsync(FurnitureUnitCreateDto furnitureUnitCreateDto);
        Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnitsAsync(FurnitureUnitFilterDto filterDto);
        Task<FurnitureUnitDetailsDto> GetFurnitureUnitDetailsAsync(Guid id);
        Task UpdateFurnitureUnitAsync(Guid id, FurnitureUnitUpdateDto furnitureUnitUpdateDto);
        Task DeleteFurnitureUnitAsync(Guid id);
        Task<List<FurnitureUnitsForDropdownDto>> GetTopCabinetFurnitureUnitsAsync();
        Task<List<FurnitureUnitsForDropdownDto>> GetBaseCabinetFurnitureUnitsAsync();
        Task<List<FurnitureUnitListByWebshopFurnitureUnitDto>> GetFurnitureUnitsByWebshopFurnitureUnitAsync(FurnitureUnitCodeFilterDto filterDto);
        Task<FurnitureUnitDetailsByWebshopFurnitureUnitDto> GetFurnitureUnitDetailsByWebshopFurnitureUnitAsync(Guid furnitureUnitId);
        Task CreateFurnitureUnitFromFileAsync(string containerName, string fileName);
        Task<FurnitureUnitForWFUDto> GetFurnitureUnitForWFUAsync(Guid id);
        Task<List<FurnitureUnitsForDropdownDto>> GetTallCabinetFurnitureUnitsAsync();
    }
}
