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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class StorageAppService : ApplicationService, IStorageAppService
    {

        private readonly IStorageRepository storageRepository;
        private readonly IFileHandlerService fileHandlerService;

        public StorageAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IStorageRepository storageRepository,
            IFileHandlerService fileHandlerService) : base(aggregate)
        {
            this.storageRepository = storageRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<int> CreateStorageAsync(StorageCreateDto storageCreateDto)
        {
            var storage = storageCreateDto.CreateModelObject();
            await storageRepository.InsertAsync(storage);
            await unitOfWork.SaveChangesAsync();

            return storage.Id;
        }

        public async Task DeleteStorageAsync(int id)
        {
            await storageRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DownloadStorageResultsAsync(StorageFilterDto storeageFilterDto)
        {
            Expression<Func<Storage, bool>> filter = CreateFilter(storeageFilterDto);

            List<Storage> storages = await storageRepository.GetAllListIncludingAsync(filter, ent => ent.Address);

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream))
                {
                    using (CsvWriter cswWriter = new CsvWriter(writer))
                    {
                        cswWriter.Configuration.Delimiter = ";";

                        cswWriter.WriteField("Name");
                        cswWriter.WriteField("Address");

                        await cswWriter.NextRecordAsync();

                        foreach (var st in storages)
                        {
                            cswWriter.WriteField(st.Name);
                            cswWriter.WriteField(st.Address.ToString());

                            await cswWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }

        public async Task<StorageDetailsDto> GetStorageAsync(int id)
        {
            var storage = await storageRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Company);
            return new StorageDetailsDto(storage);
        }

        public async Task<List<StorageNameListDto>> GetStorageNameListAsync()
        {
            var storages = await storageRepository.GetAllListAsync();
            return storages.Select(ent=>new StorageNameListDto(ent)).ToList();
        }

        public async Task<PagedListDto<StorageListDto>> GetStoragesAsync(StorageFilterDto storageFilterDto)
        {
            Expression<Func<Storage, bool>> filter = CreateFilter(storageFilterDto);

            var orderingQuery = storageFilterDto.Orderings.ToOrderingExpression<Storage>(
                StorageFilterDto.GetOrderingMapping(), nameof(Storage.Id));

            var storages = await storageRepository.GetPagedListAsync(
                filter, StorageListDto.Projection, orderingQuery, storageFilterDto.PageIndex, storageFilterDto.PageSize);

            return storages.ToPagedList();
        }

        private static Expression<Func<Storage, bool>> CreateFilter(StorageFilterDto storageFilterDto)
        {
            Expression<Func<Storage, bool>> filter = (Storage ent) => true;

            if (!string.IsNullOrWhiteSpace(storageFilterDto.Name))
            {
                filter = filter.And(e => e.Name.ToLower().Contains(storageFilterDto.Name.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(storageFilterDto.Address))
            {
                filter = filter.And(ent => (ent.Address.AddressValue + ent.Address.City + ent.Address.PostCode.ToString())
                    .ToLower().Contains(storageFilterDto.Address.ToLower().Trim()));
            }

            return filter;
        }

        public async Task UpdateStorageAsync(int id, StorageUpdateDto storageUpdateDto)
        {
            var storage = await storageRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Company);
            storage = storageUpdateDto.UpdateModelObject(storage);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
