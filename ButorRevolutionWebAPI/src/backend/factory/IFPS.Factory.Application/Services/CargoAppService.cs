using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Dto.Cargos;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CargoAppService : ApplicationService, ICargoAppService
    {
        private readonly ICargoRepository cargoRepository;
        private readonly IStockRepository stockRepository;
        private readonly ICargoStatusTypeRepository cargoStatusTypeRepository;
        private readonly IStockedMaterialRepository stockedMaterialRepository;
        private readonly IMaterialPackageRepository materialPackageRepository;
        private readonly ApplicationSettings applicationSettings;
        private readonly IFileHandlerService fileHandlerService;

        public CargoAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICargoRepository cargoRepository,
            IStockRepository stockRepository,
            ICargoStatusTypeRepository cargoStatusTypeRepository,
            IStockedMaterialRepository stockedMaterialRepository,
            IMaterialPackageRepository materialPackageRepository,
            IFileHandlerService fileHandlerService,
            IOptions<ApplicationSettings> options
            ) : base(aggregate)
        {
            this.cargoRepository = cargoRepository;
            this.stockRepository = stockRepository;
            this.cargoStatusTypeRepository = cargoStatusTypeRepository;
            this.stockedMaterialRepository = stockedMaterialRepository;
            this.materialPackageRepository = materialPackageRepository;
            this.applicationSettings = options.Value;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<int> CreateCargoFromRequiredMaterialsAsync(CargoCreateDto dto)
        {
            var newCargo = dto.CreateCargo();

            var cargoStatusType = await cargoStatusTypeRepository.SingleAsync(ent => ent.Status == CargoStatusEnum.Ordered);
            newCargo.StatusId = cargoStatusType.Id;

            foreach (var material in dto.Materials)
            {
                var materialPackage = await materialPackageRepository.SingleAsync(ent => ent.Id == material.PackageId);
                newCargo.AddOrderedPackage(material.CreateModelObject(materialPackage.Price));
                var stockedMaterial = await stockedMaterialRepository.SingleAsync(ent => ent.MaterialId == materialPackage.MaterialId);
                stockedMaterial.OrderedAmount += material.OrderedPackageNum;
                newCargo.NetCost.Value += materialPackage.Price.Value * material.OrderedPackageNum;
            }

            foreach (var additionals in dto.Additionals)
            {
                var materialPackage = await materialPackageRepository.SingleAsync(ent => ent.Id == additionals.PackageId);
                newCargo.AddOrderedPackage(additionals.CreateModelObject(materialPackage.Price));
                var stockedMaterial = await stockedMaterialRepository.SingleAsync(ent => ent.MaterialId == materialPackage.MaterialId);
                stockedMaterial.OrderedAmount += additionals.OrderedPackageNum;
                newCargo.NetCost.Value += materialPackage.Price.Value * additionals.OrderedPackageNum;
            }

            dto.SetVat(newCargo.NetCost, newCargo.Vat, double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture), newCargo.ShippingCost.CurrencyId);

            await cargoRepository.InsertAsync(newCargo);

            await unitOfWork.SaveChangesAsync();

            return newCargo.Id;
        }

        public async Task<PagedListDto<CargoListDto>> ListCargos(CargoFilterDto filterDto)
        {
            Expression<Func<Cargo, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Cargo>(CargoFilterDto.GetOrderingMapping(), nameof(Cargo.Id));

            var statuses = new List<CargoStatusEnum>();
            statuses.Add(CargoStatusEnum.WaitingForStocking);
            statuses.Add(CargoStatusEnum.Ordered);

            var allCargos = await cargoRepository.GetPagedCargoAsync(statuses, filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return allCargos.ToPagedList(CargoListDto.FromEntity);
        }


        public async Task DeleteCargoAsync(int id)
        {
            await cargoRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<CargoDetailsDto> CargoDetailsAsync(int cargoId)
        {
            var cargoFromModel = await cargoRepository.GetCargoByIdWithIncludes(cargoId);
            return new CargoDetailsDto(cargoFromModel);
        }

        public async Task<CargoDetailsPreviewDto> GetCargoDetailsForPreviewAsync(int cargoId)
        {
            var cargo = await cargoRepository.GetCargoByIdWithIncludes(cargoId);
            return new CargoDetailsPreviewDto(cargo);
        }

        public async Task UpdateProductsByCargo(int cargoId, CargoUpdateDto dto)
        {
            var cargo = await cargoRepository.GetCargoByIdWithIncludes(cargoId);

            foreach(var product in dto.Products)
            {
                var validationBuilder = new ValidationExceptionBuilder<Cargo>();

                var updatedOrderedPackage = cargo.OrderedPackages.Single(ent => ent.Id == product.Id);

                if(updatedOrderedPackage == null)
                {
                    validationBuilder.AddError(ent => ent.OrderedPackages, $"Invalid {product.Id} id value");
                }

                if(updatedOrderedPackage.MissingAmount != product.Missing)
                {
                    updatedOrderedPackage.MissingAmount = product.Missing;
                }

                if(updatedOrderedPackage.RefusedAmount != product.Refused)
                {
                    updatedOrderedPackage.RefusedAmount = product.Refused;
                }
            }

            var cargoStatusType = await cargoStatusTypeRepository.SingleAsync(ent => ent.Status == CargoStatusEnum.WaitingForStocking);
            cargo.StatusId = cargoStatusType.Id;

            cargo.ArrivedOn = Clock.Now;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<CargoDetailsWithAllInformationDto> CargoDetailsWithAllInformationAsync(int cargoId)
        {
            var cargo = await cargoRepository.GetCargoByIdWithIncludes(cargoId);
            return new CargoDetailsWithAllInformationDto(cargo, double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture));
        }

        public async Task<PagedListDto<CargoListByStockDto>> ListCargosByStock(CargoFilterDto filterDto)
        {
            Expression<Func<Cargo, bool>> filter = (Cargo c) => c.Status != null;

            if (!string.IsNullOrEmpty(filterDto.CargoName))
            {
                filter = filter.And(ent => ent.CargoName.ToLower().Contains(filterDto.CargoName.ToLower().Trim()));
            }
            if (filterDto.CargoStatusTypeId.HasValue)
            {
                filter = filter.And(ent => ent.StatusId == filterDto.CargoStatusTypeId.Value);
            }
            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date >= filterDto.From);
            }
            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date <= filterDto.To);
            }
            if (filterDto.SupplierId.HasValue)
            {
                filter = filter.And(ent => ent.SupplierId == filterDto.SupplierId.Value);
            }
            if (!string.IsNullOrEmpty(filterDto.BookedBy))
            {
                filter = filter.And(ent => ent.BookedBy.CurrentVersion.Name.ToLower().Contains(filterDto.BookedBy.ToLower().Trim()));
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Cargo>(CargoFilterDto.GetOrderingMapping(), nameof(Cargo.Id));

            var statuses = new List<CargoStatusEnum>();
            statuses.Add(CargoStatusEnum.WaitingForStocking);
            statuses.Add(CargoStatusEnum.Stocked);

            var allCargos = await cargoRepository.GetPagedCargoAsync(statuses, filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return allCargos.ToPagedList(CargoListByStockDto.FromEntity);
        }

        public async Task<CargoStockDetailsDto> GetCargoStockDetailsAsync(int cargoId)
        {
            return new CargoStockDetailsDto(await cargoRepository.GetCargoByIdWithIncludes(cargoId));
        }

        public async Task UpdateCargoStock(int cargoId, UpdateCargoStockDto dto)
        {
            var cargo = await cargoRepository.GetCargoByIdWithIncludes(cargoId);

            foreach(var package in cargo.OrderedPackages)
            {
                var updatedPackage = dto.Package.Single(ent => ent.PackageId == package.MaterialPackageId);

                var stockCreate = new StockCreateDto();
                var newStock = stockCreate.CreateModelObject(package.MaterialPackageId, updatedPackage.CellId, updatedPackage.ArrivedAmount);

                await stockRepository.InsertAsync(newStock);

                // also increase the StockedMaterial amount for the material when stocked a cargo
                var stockedMaterial = await stockedMaterialRepository.SingleAsync(ent => ent.MaterialId == package.MaterialPackage.MaterialId);
                stockedMaterial.StockedAmount += package.MaterialPackage.Size * updatedPackage.ArrivedAmount;
                stockedMaterial.OrderedAmount -= updatedPackage.ArrivedAmount;
                //TODO: check this
            }

            var cargoStatusType = await cargoStatusTypeRepository.SingleAsync(ent => ent.Status == CargoStatusEnum.Stocked);
            cargo.StatusId = cargoStatusType.Id;

            cargo.StockedOn = Clock.Now;

            await unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<Cargo, bool>> CreateFilter(CargoFilterDto filterDto)
        {
            Expression<Func<Cargo, bool>> filter = (Cargo c) => c.Status != null;

            if (!string.IsNullOrEmpty(filterDto.CargoName))
            {
                filter = filter.And(ent => ent.CargoName.ToLower().Contains(filterDto.CargoName.ToLower().Trim()));
            }
            if (filterDto.CargoStatusTypeId.HasValue)
            {
                filter = filter.And(ent => ent.StatusId == filterDto.CargoStatusTypeId.Value);
            }
            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date >= filterDto.From);
            }
            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date <= filterDto.To);
            }
            if (filterDto.SupplierId.HasValue)
            {
                filter = filter.And(ent => ent.SupplierId == filterDto.SupplierId.Value);
            }
            if (!string.IsNullOrEmpty(filterDto.BookedBy))
            {
                filter = filter.And(ent => ent.BookedBy.CurrentVersion.Name.ToLower().Contains(filterDto.BookedBy.ToLower().Trim()));
            }

            return filter;
        }

        public async Task<string> ExportCsvAsync(Stream stream, CargoFilterDto filterDto)
        {
            Expression<Func<Cargo, bool>> filter = CreateFilter(filterDto);
            var stockCargos = await cargoRepository.GetAllListIncludingAsync(filter, ent => ent.Supplier,
                                        ent => ent.BookedBy.CurrentVersion, ent => ent.Status.Translations);
            var stockCargoDatas = stockCargos.Select(ent => new CargoCsvDataDbo(ent)).ToList();
            return await fileHandlerService.ExportCsv(stockCargoDatas, "cargos", stream);
        }
    }
}
