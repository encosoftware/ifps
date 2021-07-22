using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IStorageAppService
    {
        Task<int> CreateStorageAsync(StorageCreateDto storageCreateDto);
        Task<PagedListDto<StorageListDto>> GetStoragesAsync(StorageFilterDto storageFilterDto);
        Task<StorageDetailsDto> GetStorageAsync(int id);
        Task UpdateStorageAsync(int id, StorageUpdateDto storageUpdateDto);
        Task DeleteStorageAsync(int id);
        Task<List<StorageNameListDto>> GetStorageNameListAsync();
        Task DownloadStorageResultsAsync(StorageFilterDto storageCellFilterDto);
    }
}
