using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IApplianceMaterialAppService
    {
        Task<ApplianceMaterialDetailsDto> GetApplianceMaterialAsync(Guid id);
        Task<PagedListDto<ApplianceMaterialListDto>> GetApplianceMaterialsAsync(ApplianceMaterialFilterDto filterDto);
        Task<Guid> CreateApplianceMaterialAsync(ApplianceMaterialCreateDto applianceMaterialCreateDto);
        Task UpdateApplianceMaterialAsync(Guid id, ApplianceMaterialUpdateDto applianceMaterialDto);
        Task DeleteMaterialAsync(Guid id);
        Task<List<ApplianceMaterialsListForDropdownDto>> GetApplianceMaterialsForDropdownAsync();
    }
}
