using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IFoilMaterialAppService
    {
        Task<FoilMaterialDetailsDto> GetFoilMaterialAsync(Guid id);
        Task<PagedListDto<FoilMaterialListDto>> GetFoilMaterialsAsync(FoilMaterialFilterDto filterDto);
        Task<Guid> CreateFoilMaterialAsync(FoilMaterialCreateDto foilMaterialCreateDto);
        Task UpdateFoilMaterialAsync(Guid id, FoilMaterialUpdateDto foilMaterialDto);
        Task DeleteMaterialAsync(Guid id);
        Task<List<FoilsForDropdownDto>> GetFoilsForDropdownAsync();
    }
}
