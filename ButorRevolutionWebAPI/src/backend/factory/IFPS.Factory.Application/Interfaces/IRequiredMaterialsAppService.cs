using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IRequiredMaterialsAppService
    {
        Task<PagedListDto<RequiredMaterialsListDto>> GetRequiredMaterialsListAsync(RequiredMaterialsFilterDto filterDto);
        Task<TempCargoDetailsForRequiredMaterialsDto> CreateSelectedRequiredMaterials(SelectedRequiredMaterialsDto dto);
        Task<List<MaterialCodeListDto>> GetMaterialCodesAsync();
        Task<int> CreateRequiredMaterialByOrderIdAsync(Guid orderId, int userId);
        Task<string> ExportCsvAsync(Stream stream, RequiredMaterialsFilterDto filterDto);
    }
}
