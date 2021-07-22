using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
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
    public class MaterialPackageAppService : ApplicationService, IMaterialPackageAppService
    {
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IMaterialRepository materialRepository;
        private readonly IMaterialPackageRepository materialPackageRepository;

        public MaterialPackageAppService(IApplicationServiceDependencyAggregate aggregate,
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            ICompanyRepository companyRepository,
            IMaterialRepository materialRepository,
            IMaterialPackageRepository materialPackageRepository)
            : base(aggregate)
        {
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.companyRepository = companyRepository;
            this.materialRepository = materialRepository;
            this.materialPackageRepository = materialPackageRepository;
        }

        public async Task<int> CreatMaterialPackageAsync(MaterialPackageCreateDto materialPackageCreateDto)
        {
            var materialPackage = materialPackageCreateDto.CreateModelObject();
            await materialPackageRepository.InsertAsync(materialPackage);
            await unitOfWork.SaveChangesAsync();

            return materialPackage.Id;
        }

        public async Task DeleteMaterialPackageAsync(int id)
        {
            await materialPackageRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<MaterialPackageDetailsDto> GeMaterialPackageAsync(int id)
        {
            var matPack = await materialPackageRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Price.Currency);
            return new MaterialPackageDetailsDto(matPack);
        }

        public async Task<PagedListDto<MaterialPackageListDto>> GetMaterialPackagesAsync(MaterialPackageFilterDto filterDto)
        {
            Expression<Func<MaterialPackage, bool>> filter = (MaterialPackage ent) => true;

            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.PackageCode.ToLower().Contains(filterDto.Code.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Description))
                {
                    filter = filter.And(ent => ent.PackageDescription.ToLower().Contains(filterDto.Description.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.SupplierName))
                {
                    filter = filter.And(ent => ent.Supplier.Name.ToLower().Contains(filterDto.SupplierName.ToLower()));
                }

                if (filterDto.Size != 0)

                {
                    filter = filter.And(ent => ent.Size == filterDto.Size);
                }
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<MaterialPackage>(MaterialPackageFilterDto.GetOrderingMapping(), nameof(WorktopBoardMaterial.Id));

            var materialPackages = await materialPackageRepository.GetMaterialPackagePagedListAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return materialPackages.ToPagedList(MaterialPackageListDto.FromEntity);
        }

        public async Task<List<MaterialPackageCodeListDto>> GetPackageCodes()
        {
            var packages = await materialPackageRepository.GetAllListAsync();
            return packages.Select(ent => new MaterialPackageCodeListDto(ent)).ToList();
        }

        public async Task ImportMaterialPackagesFromCsv(MaterialPackageImportDto materialPackageImportDto)
        {
            await CreateMaterialPackagesFromCsv(materialPackageImportDto.ContainerName, materialPackageImportDto.AccessoryCsvFileName);
            await CreateMaterialPackagesFromCsv(materialPackageImportDto.ContainerName, materialPackageImportDto.ApplianceCsvFileName);
            await CreateMaterialPackagesFromCsv(materialPackageImportDto.ContainerName, materialPackageImportDto.DecorBoardCsvFileName);
            await CreateMaterialPackagesFromCsv(materialPackageImportDto.ContainerName, materialPackageImportDto.FoilCsvFileName);
            await CreateMaterialPackagesFromCsv(materialPackageImportDto.ContainerName, materialPackageImportDto.WorktopBoardCsvFileName);

            await unitOfWork.SaveChangesAsync();
        }

        private async Task CreateMaterialPackagesFromCsv(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);
            var currencies = await currencyRepository.GetAllListAsync();
            string[] lines = System.IO.File.ReadAllLines(url);
            var codes = new List<string>();

            foreach (string line in lines)
            {
                string[] data = line.Split(';');
                if (data.Length != 5 || !currencies.Any(ent => ent.Name.Equals(data[2])) || codes.Any(ent => ent.Equals(data[0])))
                {
                    continue;
                }

                var existingMaterial = await materialRepository.SingleOrDefaultAsync(ent => ent.Code == data[0]);
                if (existingMaterial == null)
                {
                    continue;
                }

                var price = new Price(double.Parse(data[1]), currencies.Single(ent => ent.Name.Equals(data[2])).Id);
                var company = await companyRepository.SingleAsync(ent => ent.Id == int.Parse(data[3]));
                var materialPackage = new MaterialPackage(existingMaterial.Id, company.Id, price) { Size = int.Parse(data[4]), PackageCode = $"{data[0]} package" };
                await materialPackageRepository.InsertAsync(materialPackage);
                codes.Add(data[0]);
            }
        }


        public async Task UpdateMaterialPackageAsync(int id, MaterialPackageUpdateDto materialPackageUpdateDto)
        {
            var materialPackage = await materialPackageRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Price.Currency);
            materialPackage = materialPackageUpdateDto.UpdateModelObject(materialPackage);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
