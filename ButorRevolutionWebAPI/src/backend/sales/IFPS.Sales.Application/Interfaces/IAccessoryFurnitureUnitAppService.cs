using IFPS.Sales.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IAccessoryFurnitureUnitAppService
    {
        Task<int> CreateAccessoryFurnitureUnitAsync(AccessoryFurnitureUnitCreateDto accessoryComponentCreateDto);
        Task<AccessoryFurnitureUnitDetailsDto> GetAccessoryFurnitureUnitDetailsAsync(int id);
        Task UpdateAccessoryFurnitureUnitAsync(int id, AccessoryFurnitureUnitUpdateDto accessoryComponentUpdateDto);
        Task DeleteAccessoryFurnitureUnitAsync(int id);
    }
}
