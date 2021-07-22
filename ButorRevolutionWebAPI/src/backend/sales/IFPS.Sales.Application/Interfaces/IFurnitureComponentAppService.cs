using IFPS.Sales.Application.Dto;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IFurnitureComponentAppService
    {
        Task<Guid> CreateFurnitureComponentAsync(FurnitureComponentCreateDto furnitureComponentCreateDto);
        Task<FurnitureComponentDetailsDto> GetFurnitureComponentDetailsAsync(Guid id);        
        Task UpdateFurnitureComponentAsync(Guid id, FurnitureComponentUpdateDto furnitureComponentUpdateDto);
        Task DeleteFurnitureComponentAsync(Guid id);
    }
}
