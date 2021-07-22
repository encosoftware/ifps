using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IMaterialPackageAppService
    {
        Task<List<MaterialPackageCodeListDto>> GetPackageCodes();
        Task<MaterialPackageDetailsDto> GeMaterialPackageAsync(int id);
        Task<PagedListDto<MaterialPackageListDto>> GetMaterialPackagesAsync(MaterialPackageFilterDto filterDto);
        Task<int> CreatMaterialPackageAsync(MaterialPackageCreateDto materialPackageCreateDto);
        Task UpdateMaterialPackageAsync(int id, MaterialPackageUpdateDto materialPackageDto);
        Task DeleteMaterialPackageAsync(int id);
        Task ImportMaterialPackagesFromCsv(MaterialPackageImportDto materialPackageImportDto);
    }
}
