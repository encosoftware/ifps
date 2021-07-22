using CsvHelper;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class StorageCellAppService : ApplicationService, IStorageCellAppService
    {

        private readonly IStorageCellRepository storageCellRepository;
        private readonly IFileHandlerService fileHandlerService;

        public StorageCellAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IStorageCellRepository storageCellRepository,
            IFileHandlerService fileHandlerService) : base(aggregate)
        {
            this.storageCellRepository = storageCellRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<int> CreateStorageCellAsync(StorageCellCreateDto storageCellCreateDto)
        {
            var storageCell = storageCellCreateDto.CreateModelObject();
            await storageCellRepository.InsertAsync(storageCell);
            await unitOfWork.SaveChangesAsync();

            return storageCell.Id;
        }

        public async Task DeleteStorageCellAsync(int id)
        {
            await storageCellRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<StorageCellDetailsDto> GetStorageCellAsync(int id)
        {
            var storageCell = await storageCellRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Warehouse);
            return new StorageCellDetailsDto(storageCell);
        }

        public async Task<PagedListDto<StorageCellListDto>> GetStorageCellsAsync(StorageCellFilterDto storageCellFilterDto)
        {
            Expression<Func<StorageCell, bool>> filter = CreateFilter(storageCellFilterDto);

            var orderingQuery = storageCellFilterDto.Orderings.ToOrderingExpression<StorageCell>(
                StorageCellFilterDto.GetOrderingMapping(), nameof(StorageCell.Id));

            var storageCells = await storageCellRepository.GetPagedListAsync(
                filter, StorageCellListDto.Projection, orderingQuery, storageCellFilterDto.PageIndex, storageCellFilterDto.PageSize);

            return storageCells.ToPagedList();
        }

        private static Expression<Func<StorageCell, bool>> CreateFilter(StorageCellFilterDto storageCellFilterDto)
        {
            Expression<Func<StorageCell, bool>> filter = (StorageCell ent) => true;

            if (!string.IsNullOrWhiteSpace(storageCellFilterDto.Name))
            {
                filter = filter.And(e => e.Name.ToLower().Contains(storageCellFilterDto.Name.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(storageCellFilterDto.Description))
            {
                filter = filter.And(e => e.Metadata.ToLower().Contains(storageCellFilterDto.Description.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(storageCellFilterDto.StorageName))
            {
                filter = filter.And(e => e.Warehouse.Name.ToLower().Contains(storageCellFilterDto.StorageName.ToLower().Trim()));
            }

            return filter;
        }

        public async Task UpdateStorageCellAsync(int id, StorageCellUpdateDto storageCellUpdateDto)
        {
            var storageCell = await storageCellRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Warehouse);
            storageCell = storageCellUpdateDto.UpdateModelObject(storageCell);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<StorageCellDropdownListDto>> ListDropDownStorageCellsAsync()
        {
            var storageCells = await storageCellRepository.GetAllListAsync();

            var storageCellList = new List<StorageCellDropdownListDto>();

            foreach (var cell in storageCells)
            {
                var storageCell = new StorageCellDropdownListDto(cell.Id, cell.Name);
                storageCellList.Add(storageCell);
            }

            return storageCellList;
        }

        public async Task DownloadStorageCellResultsAsync(StorageCellFilterDto storageCellFilterDto)
        {
            Expression<Func<StorageCell, bool>> filter = CreateFilter(storageCellFilterDto);

            List<StorageCell> storageCells = await storageCellRepository.GetAllListIncludingAsync(filter, ent => ent.Warehouse);


            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream))
                {
                    using (CsvWriter cswWriter = new CsvWriter(writer))
                    {
                        cswWriter.Configuration.Delimiter = ";";

                        cswWriter.WriteField("Name");
                        cswWriter.WriteField("Stock");
                        cswWriter.WriteField("Description");

                        await cswWriter.NextRecordAsync();

                        foreach (var cell in storageCells)
                        {
                            cswWriter.WriteField(cell.Name);
                            cswWriter.WriteField(cell.Warehouse.Name);
                            cswWriter.WriteField(cell.Metadata);

                            await cswWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }
    }
}