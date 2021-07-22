using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IStockAppService
    {
        Task<int> CreateStockAsync(StockCreateDto stockCreateDto);
        Task<PagedListDto<StockListDto>> GetStocksAsync(StockFilterDto stockFilterDto);
        Task<StockDetailsDto> GetStockAsync(int id);
        Task UpdateStockAsync(int id, StockUpdateDto stockUpdateDto);
        Task DeleteStockAsync(int id);
        Task ReserveStocksAsync(StockReserveDto stockReserveDto);
        Task EjectStocksAsync(List<StockQuantityDto> stockEjectDtos);
        Task DownloadStockResultsAsync(StockFilterDto stockFilterDto);
        Task<string> ExportCsvAsync(Stream stream, StockFilterDto filterDto);
    }
}
