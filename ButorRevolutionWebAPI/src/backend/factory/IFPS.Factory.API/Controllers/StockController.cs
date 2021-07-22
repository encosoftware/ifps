using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : IFPSControllerBase
    {
        private const string OPNAME = "Stocks";
        private readonly IStockAppService service;

        public StockController(IStockAppService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateStockAsync([FromBody] StockCreateDto stockCreateDto)
        {
            return service.CreateStockAsync(stockCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<StockListDto>> GetStocksAsync([FromQuery] StockFilterDto stockFilterDto)
        {
            return service.GetStocksAsync(stockFilterDto);
        }

        [HttpGet("download")]
        [Authorize(Policy = "GetStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DownloadStocksAsync([FromQuery] StockFilterDto stockFilterDto)
        {
            await service.DownloadStockResultsAsync(stockFilterDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<StockDetailsDto> GetStockAsync(int id)
        {
            return service.GetStockAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateStockAsync(int id, [FromBody] StockUpdateDto stockUpdateDto)
        {
            return service.UpdateStockAsync(id, stockUpdateDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteStockAsync(int id)
        {
            return service.DeleteStockAsync(id);
        }

        [HttpPost("eject")]
        [Authorize(Policy = "UpdateStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task EjectStockAsync([FromBody] List<StockQuantityDto> stockEjectDtos)
        {
            return service.EjectStocksAsync(stockEjectDtos);
        }

        [HttpPost("reserve")]
        [Authorize(Policy = "UpdateStocks")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ReserveStockAsync([FromBody] StockReserveDto stockReserveDto)
        {
            return service.ReserveStocksAsync(stockReserveDto);
        }

        // GET export csv file
        [HttpGet("export")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> ExportCsv([FromQuery]StockFilterDto filterDto)
        {
            var stream = new MemoryStream();
            var csv = await service.ExportCsvAsync(stream, filterDto);
            return File(stream, "application/octet-stream", csv);
        }
    }
}
