using ENCO.DDD;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Application.Exceptions;

namespace IFPS.Sales.Application.Services
{
    public class FurnitureUnitAppService : ApplicationService, IFurnitureUnitAppService
    {
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;
        private readonly IFoilMaterialRepository foilMaterialRepository;
        private readonly IAccessoryMaterialRepository accessoryMaterialRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IFurnitureUnitTypeRepository furnitureUnitTypeRepository;

        public FurnitureUnitAppService(IApplicationServiceDependencyAggregate aggregate,
           IFurnitureUnitRepository furnitureUnitRepository,
           IGroupingCategoryRepository groupingCategoryRepository,
           IFileHandlerService fileHandlerService,
           IFoilMaterialRepository foilMaterialRepository,
           IDecorBoardMaterialRepository decorBoardMaterialRepository,
           IAccessoryMaterialRepository accessoryMaterialRepository,
           ICurrencyRepository currencyRepository,
           IFurnitureUnitTypeRepository furnitureUnitTypeRepository
           )
            : base(aggregate)
        {
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.fileHandlerService = fileHandlerService;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
            this.foilMaterialRepository = foilMaterialRepository;
            this.accessoryMaterialRepository = accessoryMaterialRepository;
            this.currencyRepository = currencyRepository;
            this.furnitureUnitTypeRepository = furnitureUnitTypeRepository;
        }

        public async Task<Guid> CreateFurnitureUnitAsync(FurnitureUnitCreateDto furnitureUnitCreateDto)
        {
            if (await furnitureUnitRepository.IsFurnitureUnitExistedAsync(furnitureUnitCreateDto.Code))
            {
                new ValidationExceptionBuilder().AddError(nameof(furnitureUnitCreateDto.Code), "Furniture unit already exists with this code!").ThrowIfHasError();
            }

            var furnitureUnit = furnitureUnitCreateDto.CreateModelObject();
            furnitureUnit.ImageId = await fileHandlerService.InsertImage(furnitureUnitCreateDto.ImageCreateDto.ContainerName, furnitureUnitCreateDto.ImageCreateDto.FileName);

            furnitureUnitCreateDto.SetDefaultPrices(furnitureUnit);

            await furnitureUnitRepository.InsertAsync(furnitureUnit);
            await unitOfWork.SaveChangesAsync();
            return furnitureUnit.Id;
        }

        public async Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnitsAsync(FurnitureUnitFilterDto filterDto)
        {
            Expression<Func<FurnitureUnit, bool>> filter = (FurnitureUnit c) => c.Code != null;

            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.Code.Contains(filterDto.Code));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Description))
                {
                    filter = filter.And(ent => ent.Description.Contains(filterDto.Description));
                }

                if (filterDto.CategoryId != 0)
                {
                    filter = filter.And(ent => ent.CategoryId.Equals(filterDto.CategoryId));
                }
            }

            var columnMappings = new Dictionary<string, string>
            {
                { nameof(filterDto.Code), nameof(FurnitureUnit.Code) },
                { nameof(filterDto.Description), nameof(FurnitureUnit.Description) }
            };
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<FurnitureUnit>(columnMappings, nameof(FurnitureUnit.Id));

            var furnitureUnits = await furnitureUnitRepository.GetFurnitureUnitsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return furnitureUnits.ToPagedList(FurnitureUnitListDto.FromEntity);
        }

        public async Task<FurnitureUnitDetailsDto> GetFurnitureUnitDetailsAsync(Guid id)
        {
            var furnitureUnit = await furnitureUnitRepository.GetFurnitureUnitAsync(id);
            Ensure.NotNull(furnitureUnit);
            return new FurnitureUnitDetailsDto(furnitureUnit);
        }

        public async Task UpdateFurnitureUnitAsync(Guid id, FurnitureUnitUpdateDto furnitureUnitUpdateDto)
        {
            var furnitureUnit = await furnitureUnitRepository.GetFurnitureUnitAsync(id);
            Ensure.NotNull(furnitureUnit);
            furnitureUnit = furnitureUnitUpdateDto.UpdateModelObject(furnitureUnit);
            furnitureUnit.Image = await fileHandlerService.UpdateImage(furnitureUnit.ImageId, furnitureUnitUpdateDto.ImageUpdateDto.ContainerName, furnitureUnitUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteFurnitureUnitAsync(Guid id)
        {
            await furnitureUnitRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FurnitureUnitsForDropdownDto>> GetTopCabinetFurnitureUnitsAsync()
        {
            var furnitureUnits = await furnitureUnitRepository.GetFurnitureUnitsByType(FurnitureUnitTypeEnum.Top);
            return furnitureUnits.Select(ent => new FurnitureUnitsForDropdownDto(ent)).ToList();
        }

        public async Task<List<FurnitureUnitsForDropdownDto>> GetBaseCabinetFurnitureUnitsAsync()
        {
            var furnitureUnits = await furnitureUnitRepository.GetFurnitureUnitsByType(FurnitureUnitTypeEnum.Base);
            return furnitureUnits.Select(ent => new FurnitureUnitsForDropdownDto(ent)).ToList();
        }

        public async Task<List<FurnitureUnitsForDropdownDto>> GetTallCabinetFurnitureUnitsAsync()
        {
            var furnitureUnits = await furnitureUnitRepository.GetFurnitureUnitsByType(FurnitureUnitTypeEnum.Tall);
            return furnitureUnits.Select(ent => new FurnitureUnitsForDropdownDto(ent)).ToList();
        }

        public async Task<FurnitureUnitDetailsByWebshopFurnitureUnitDto> GetFurnitureUnitDetailsByWebshopFurnitureUnitAsync(Guid furnitureUnitId)
        {
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureUnitId, ent => ent.CurrentPrice.Price.Currency, ent => ent.Image);
            return new FurnitureUnitDetailsByWebshopFurnitureUnitDto(furnitureUnit);
        }

        public async Task<List<FurnitureUnitListByWebshopFurnitureUnitDto>> GetFurnitureUnitsByWebshopFurnitureUnitAsync(FurnitureUnitCodeFilterDto filterDto)
        {
            Expression<Func<FurnitureUnit, bool>> filter = (FurnitureUnit ent) => ent.Code.Contains(filterDto.Code, StringComparison.InvariantCultureIgnoreCase);

            var furnitureUnits = await furnitureUnitRepository.GetAllListAsync(filter);
            return furnitureUnits.Select(ent => new FurnitureUnitListByWebshopFurnitureUnitDto(ent)).ToList();
        }

        public async Task CreateFurnitureUnitFromFileAsync(string containerName, string fileName)
        {
            var url = fileHandlerService.GetFileUrl(containerName, fileName);

            string[] lines = System.IO.File.ReadAllLines(url);

            string[] firstLine = lines[0].Split(';');
            var currency = await currencyRepository.SingleAsync(ent => ent.Name.Equals(firstLine[9]));

            var furnitureUnit = new FurnitureUnit(firstLine[0],
                Double.Parse(firstLine[3], CultureInfo.InvariantCulture),
                Double.Parse(firstLine[4], CultureInfo.InvariantCulture),
                Double.Parse(firstLine[5], CultureInfo.InvariantCulture));
            furnitureUnit.Description = firstLine[1];
            furnitureUnit.AddPrice(new FurnitureUnitPrice()
            {
                Price = new Price(Double.Parse(firstLine[8]), currency.Id),
                MaterialCost = new Price(0.8 * Double.Parse(firstLine[8]), currency.Id),
                ValidFrom = Clock.Now,
            });

            string imgDirPath = firstLine[6];
            string unitImgFilename = firstLine[7];
            if (imgDirPath == "" || unitImgFilename == "")
            {
                throw new IFPSDomainException("Empty file or directory string in the csv file!");
            }

            // TODO - separate thumbnail files?
            var unitImg = new Image(Path.Combine(imgDirPath, unitImgFilename), ".png", "FurnitureUnits")
            { ThumbnailName = Path.Combine(imgDirPath, unitImgFilename) };

            furnitureUnit.Image = unitImg;

            var enumType = (FurnitureUnitTypeEnum)Enum.Parse(typeof(FurnitureUnitTypeEnum), firstLine[2], true);
            var furnitureUnitType = await furnitureUnitTypeRepository.SingleAsync(ent => ent.Type == enumType);
            furnitureUnit.FurnitureUnitTypeId = furnitureUnitType.Id;

            var category = await groupingCategoryRepository.GetGroupingCategoryByName(firstLine[2]);
            furnitureUnit.Category = category ?? throw new IFPSDomainException("GroupingCategory not found for FurnitureUnit!");

            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(';');

                // add furniture component
                if (data[1] != "")
                {
                    var furnitureComponent = new FurnitureComponent(data[2],
                        Double.Parse(data[6], CultureInfo.InvariantCulture),
                        Double.Parse(data[5], CultureInfo.InvariantCulture),
                        int.Parse(data[3]));

                    var boardMaterial = await decorBoardMaterialRepository.SingleAsync(ent => ent.Code == data[0]);
                    furnitureComponent.BoardMaterial = boardMaterial;

                    if (data[7] != "")
                    {
                        var topFoil = await foilMaterialRepository.SingleAsync(ent => ent.Code == data[7]);
                        furnitureComponent.TopFoil = topFoil;
                    }
                    if (data[8] != "")
                    {
                        var bottomFoil = await foilMaterialRepository.SingleAsync(ent => ent.Code == data[8]);
                        furnitureComponent.BottomFoil = bottomFoil;
                    }
                    if (data[9] != "")
                    {
                        var leftFoil = await foilMaterialRepository.SingleAsync(ent => ent.Code == data[9]);
                        furnitureComponent.LeftFoil = leftFoil;
                    }
                    if (data[10] != "")
                    {
                        var rightFoil = await foilMaterialRepository.SingleAsync(ent => ent.Code == data[10]);
                        furnitureComponent.RightFoil = rightFoil;
                    }

                    furnitureComponent.Type = FurnitureComponentTypeEnum.Corpus;

                    string componentImgFilename = data[11];
                    if (componentImgFilename != "")
                    {
                        // TODO - separate thumbnail files?
                        var componentImg = new Image(Path.Combine(imgDirPath, componentImgFilename), ".png", "FurnitureUnits")
                        { ThumbnailName = Path.Combine(imgDirPath, componentImgFilename) };

                        furnitureComponent.Image = componentImg;
                    }
                    else
                    {
                        furnitureComponent.ImageId = Guid.Parse("d9bd4a0d-47b9-4188-90c7-beae54626523");
                    }

                    furnitureUnit.AddFurnitureComponent(furnitureComponent);

                }
                // add accessory
                else
                {
                    var accessory = await accessoryMaterialRepository.SingleAsync(ent => ent.Code == data[0]);

                    var accessoryFurnitureUnit = new AccessoryMaterialFurnitureUnit("", int.Parse(data[3], NumberStyles.AllowDecimalPoint));
                    accessoryFurnitureUnit.Accessory = accessory;
                    accessoryFurnitureUnit.FurnitureUnit = furnitureUnit;
                    furnitureUnit.AddAccessory(accessoryFurnitureUnit);
                }
            }

            await furnitureUnitRepository.InsertAsync(furnitureUnit);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<FurnitureUnitForWFUDto> GetFurnitureUnitForWFUAsync(Guid id)
        {
            return await furnitureUnitRepository.SingleAsync(ent => ent.Id == id, FurnitureUnitForWFUDto.Projection);
        }
    }
}
