using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IAccessoryMaterialAppService
    {
        Task<AccessoryMaterialDetailsDto> GetAccessoryMaterialAsync(Guid id);
        Task<PagedListDto<AccessoryMaterialListDto>> GetAccessoryMaterialsAsync(AccessoryMaterialFilterDto filterDto);
        Task<Guid> CreateAccessoryMaterialAsync(AccessoryMaterialCreateDto accessoryMaterialCreateDto);
        Task UpdateAccessoryMaterialAsync(Guid id, AccessoryMaterialUpdateDto accessoryMaterialDto);
        Task DeleteMaterialAsync(Guid id);
        Task<List<AccessoryMaterialCodesDto>> GetAccessoryMaterialCodesAsync();
    }
}
