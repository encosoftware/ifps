using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IStorageCellAppService
    {
        Task<int> CreateStorageCellAsync(StorageCellCreateDto storageCellCreateDto);
        Task<PagedListDto<StorageCellListDto>> GetStorageCellsAsync(StorageCellFilterDto storageCellFilterDto);
        Task<StorageCellDetailsDto> GetStorageCellAsync(int id);
        Task UpdateStorageCellAsync(int id, StorageCellUpdateDto storageCellUpdateDto);
        Task DeleteStorageCellAsync(int id);
        Task<List<StorageCellDropdownListDto>> ListDropDownStorageCellsAsync();
        Task DownloadStorageCellResultsAsync(StorageCellFilterDto storageCellFilterDto);
    }
}
