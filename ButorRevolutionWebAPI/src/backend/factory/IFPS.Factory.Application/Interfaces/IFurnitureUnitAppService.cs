using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IFurnitureUnitAppService
    {
        Task CreateFurnitureUnitFromFileAsync(FurnitureUnitDetailsFromFileDto dto, string containerName, string fileName);
        Task<List<FurnitureUnitForDataGenerationListDto>> GetFurnitureUnitsForDataGenerationAsync();
        Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnitsAsync(FurnitureUnitFilterDto dto);
        Task ParseXxlDataForCncGeneration(Guid furnitureUnitId, string containerName, string fileName);
    }
}
