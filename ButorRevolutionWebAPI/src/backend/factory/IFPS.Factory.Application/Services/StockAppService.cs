using CsvHelper;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Exceptions;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class StockAppService : ApplicationService, IStockAppService
    {
        private readonly IStockRepository stockRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IFileHandlerService fileHandlerService;

        public StockAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IStockRepository stockRepository,
            IOrderRepository orderRepository,
            IFileHandlerService fileHandlerService) : base(aggregate)
        {
            this.stockRepository = stockRepository;
            this.orderRepository = orderRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<int> CreateStockAsync(StockCreateDto stockCreateDto)
        {
            var stock = stockCreateDto.CreateModelObject();
            await stockRepository.InsertAsync(stock);
            await unitOfWork.SaveChangesAsync();

            return stock.Id;
        }

        public async Task DeleteStockAsync(int id)
        {
            await stockRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DownloadStockResultsAsync(StockFilterDto stockFilterDto)
        {
            Expression<Func<Stock, bool>> filter = CreateFilter(stockFilterDto);

            List<StockFilterDto> stocks = await stockRepository.GetAllListAsync(filter, StockFilterDto.GetProjection());

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream))
                {
                    using (CsvWriter cswWriter = new CsvWriter(writer))
                    {
                        cswWriter.Configuration.Delimiter = ";";

                        cswWriter.WriteField("Description");
                        cswWriter.WriteField("Code");
                        cswWriter.WriteField("Cell Name");
                        cswWriter.WriteField("Cell Metadata");
                        cswWriter.WriteField("Cell Size");

                        await cswWriter.NextRecordAsync();

                        foreach (var s in stocks)
                        {
                            cswWriter.WriteField(s.PackageDescription);
                            cswWriter.WriteField(s.PackageCode);
                            cswWriter.WriteField(s.StorageCellName);
                            cswWriter.WriteField(s.StorageCellMetadata);
                            cswWriter.WriteField(s.Quantity);

                            await cswWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }

        public async Task EjectStocksAsync(List<StockQuantityDto> stockEjectDtos)
        {
            var stocks = await stockRepository.GetAllListAsync(ent => stockEjectDtos.Select(x => x.Id).Contains(ent.Id) && ent.ValidTo == null);
            foreach (var stock in stocks)
            {
                var stockDto = stockEjectDtos.Single(ent => ent.Id == stock.Id);
                if (stockDto.Quantity > stock.Quantity)
                {
                    throw new IFPSDomainException("Ejected stock quantity is not valid");
                }
                stock.Quantity -= stockDto.Quantity;
                var newStock = new Stock(stock.PackageId, stockDto.Quantity, stock.StorageCellId) { ValidTo = Clock.Now };

                await stockRepository.InsertAsync(newStock);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<StockDetailsDto> GetStockAsync(int id)
        {
            var stock = await stockRepository.SingleAsync(ent => ent.Id == id && ent.ValidTo == null);
            return new StockDetailsDto(stock);
        }

        public async Task<PagedListDto<StockListDto>> GetStocksAsync(StockFilterDto stockFilterDto)
        {
            Expression<Func<Stock, bool>> filter = CreateFilter(stockFilterDto);

            var orderingQuery = stockFilterDto.Orderings.ToOrderingExpression<Stock>(
                StockFilterDto.GetOrderingMapping(), nameof(Stock.Id));

            var stocks = await stockRepository.GetPagedListAsync(
                filter, StockListDto.Projection, orderingQuery, stockFilterDto.PageIndex, stockFilterDto.PageSize);

            return stocks.ToPagedList();
        }

        private static Expression<Func<Stock, bool>> CreateFilter(StockFilterDto stockFilterDto)
        {
            Expression<Func<Stock, bool>> filter = (Stock ent) => ent.ValidTo == null;

            if (!string.IsNullOrWhiteSpace(stockFilterDto.PackageCode))
            {
                filter = filter.And(ent => ent.Package.PackageCode.ToLower().Contains(stockFilterDto.PackageCode.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(stockFilterDto.PackageDescription))
            {
                filter = filter.And(ent => ent.Package.PackageDescription.ToLower().Contains(stockFilterDto.PackageDescription.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(stockFilterDto.StorageCellName))
            {
                filter = filter.And(ent => ent.StorageCell.Name.ToLower().Contains(stockFilterDto.StorageCellName.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(stockFilterDto.StorageCellMetadata))
            {
                filter = filter.And(ent => ent.StorageCell.Metadata.ToLower().Contains(stockFilterDto.StorageCellMetadata.ToLower().Trim()));
            }

            if (stockFilterDto.Quantity != 0)
            {
                filter = filter.And(ent => ent.Quantity.Equals(stockFilterDto.Quantity));
            }

            return filter;
        }

        public async Task ReserveStocksAsync(StockReserveDto stockReserveDto)
        {
            var stocks = await stockRepository.GetAllListAsync(ent => stockReserveDto.StockQuantities.Select(x => x.Id).Contains(ent.Id) && ent.ValidTo == null);
            var order = await orderRepository.SingleAsync(ent => ent.Id == stockReserveDto.OrderId);
            foreach (var stock in stocks)
            {
                var stockDto = stockReserveDto.StockQuantities.Single(ent => ent.Id == stock.Id);
                if (stockDto.Quantity > stock.Quantity)
                {
                    throw new IFPSDomainException("Reserved stock quantity is not valid");
                }
                stock.Quantity -= stockDto.Quantity;
                if (stock.Quantity == 0)
                {
                    stock.ValidTo = Clock.Now;
                }
                var newStock = new Stock(stock.PackageId, stockDto.Quantity, stock.StorageCellId); // { OrderId = order.Id, OrderName = order.OrderName };

                await stockRepository.InsertAsync(newStock);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(int id, StockUpdateDto stockUpdateDto)
        {
            var stock = await stockRepository.SingleAsync(ent => ent.Id == id && ent.ValidTo == null);
            stock = stockUpdateDto.UpdateModelObject(stock);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<string> ExportCsvAsync(Stream stream, StockFilterDto filterDto)
        {
            Expression<Func<Stock, bool>> filter = CreateFilter(filterDto);
            var stockedItems = await stockRepository.GetAllListIncludingAsync(filter, ent => ent.Package, ent => ent.StorageCell);
            var stockedItemDatas = stockedItems.Select(ent => new StockedItemCsvDataDbo(ent)).ToList();
            return await fileHandlerService.ExportCsv(stockedItemDatas, "stockedItems", stream);
        }
    }
}