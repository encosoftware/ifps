using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class FurnitureUnitAppService : ApplicationService, IFurnitureUnitAppService
    {
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IFurnitureComponentRepository furnitureComponentRepository;
        private readonly IAccessoryFurnitureUnitRepository accessoryFurnitureUnitRepository;
        private readonly IBoardMaterialRepository boardMaterialRepository;
        private readonly IFoilMaterialRepository foilMaterialRepository;
        private readonly IAccessoryMaterialRepository accessoryMaterialRepository;
        private readonly IMaterialRepository materialRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ITriDCorpusComponentLoaderService triDCorpusComponentLoader;

        public FurnitureUnitAppService(IApplicationServiceDependencyAggregate aggregate,
           IFurnitureUnitRepository furnitureUnitRepository,
           IFurnitureComponentRepository furnitureComponentRepository,
           IAccessoryFurnitureUnitRepository accessoryFurnitureUnitRepository,
           IBoardMaterialRepository boardMaterialRepository,
           IFoilMaterialRepository foilMaterialRepository,
           IAccessoryMaterialRepository accessoryMaterialRepository,
           IMaterialRepository materialRepository,
           IFileHandlerService fileHandlerService,
           IHostingEnvironment hostingEnvironment,
           ITriDCorpusComponentLoaderService triDCorpusComponentLoader)
            : base(aggregate)
        {
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.fileHandlerService = fileHandlerService;
            this.furnitureComponentRepository = furnitureComponentRepository;
            this.accessoryFurnitureUnitRepository = accessoryFurnitureUnitRepository;
            this.boardMaterialRepository = boardMaterialRepository;
            this.foilMaterialRepository = foilMaterialRepository;
            this.accessoryMaterialRepository = accessoryMaterialRepository;
            this.materialRepository = materialRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.triDCorpusComponentLoader = triDCorpusComponentLoader;
        }

        public async Task CreateFurnitureUnitFromFileAsync(FurnitureUnitDetailsFromFileDto dto, string containerName, string fileName)
        {
            var path = containerName + fileName;

            using (var reader = new StreamReader(path))
            {
                var furnitureUnit = new FurnitureUnit(Path.GetFileNameWithoutExtension(fileName));
                furnitureUnit.ImageId = furnitureUnit.ImageId = await fileHandlerService.InsertImage(dto.Image.ContainerName, dto.Image.FileName);
                dto.SetSize(furnitureUnit);
                await furnitureUnitRepository.InsertAsync(furnitureUnit);

                var furnitureUnitPrice = new FurnitureUnitPrice() { MaterialCost = dto.MaterialCost.CreateModelObject(), Price = dto.Price.CreateModelObject() };
                furnitureUnit.AddPrice(furnitureUnitPrice);

                var headerLine = reader.ReadLine();
                var materialCodes = new HashSet<string>();
                FurnitureComponent furnitureComponent = null;
                var components = new List<FurnitureComponent>();
                var foilsArrayInList = new List<string[]>();

                int i = 0;

                while (!reader.EndOfStream)
                {
                    var record = reader.ReadLine();
                    var values = record.Split(";");

                    if (!string.IsNullOrEmpty(values[7]) || !string.IsNullOrEmpty(values[8]) || !string.IsNullOrEmpty(values[9]) || !string.IsNullOrEmpty(values[10]))
                    {
                        foilsArrayInList.Add(new string[] { values[7], values[8], values[9], values[10] });
                    }

                    if (!string.IsNullOrEmpty(values[2]))
                    {
                        furnitureComponent = new FurnitureComponent(values[2], double.Parse(values[6], CultureInfo.InvariantCulture), double.Parse(values[4], CultureInfo.InvariantCulture), (int)double.Parse(values[3], CultureInfo.InvariantCulture)) { Length = double.Parse(values[5], CultureInfo.InvariantCulture) };
                        furnitureComponent.FurnitureUnitId = furnitureUnit.Id;
                        var tempPoint = new TempAbsolutePoint();
                        furnitureComponent.CenterPoint = tempPoint.CreateModelObject();
                    }

                    var existingMaterial = await materialRepository.SingleOrDefaultAsync(ent => ent.Code == values[0]);
                    if (!string.IsNullOrEmpty(values[2]))
                    {
                        if (existingMaterial == null && !materialCodes.Contains(values[0]))
                        {
                            var boardMaterial = new BoardMaterial(values[0]);
                            boardMaterial.Description = values[1];
                            boardMaterial.Dimension = new Dimension(4000, 2700, 18);
                            await boardMaterialRepository.InsertAsync(boardMaterial);

                            furnitureComponent.BoardMaterialId = boardMaterial.Id;
                        }
                        else if (existingMaterial != null)
                        {
                            furnitureComponent.BoardMaterialId = existingMaterial.Id;
                        }
                        else
                        {
                            // TODO ezt nem tudom használjuk-e még egyáltalán, de kikommenteltem
                            //furnitureComponent.BoardMaterialId = boardMaterial.Id;
                        }

                        materialCodes.Add(values[0]);

                        if (!string.IsNullOrEmpty(values[11]))
                        {
                            var type = (ComponentPositionTypeEnum)Enum.Parse(typeof(ComponentPositionTypeEnum), values[11]);
                            furnitureComponent.PositionType = type;
                        }

                        components.Add(furnitureComponent);
                        await furnitureComponentRepository.InsertAsync(furnitureComponent);
                    }
                    else
                    {
                        if (existingMaterial == null)
                        {
                            if (double.Parse(values[4], CultureInfo.InvariantCulture) == 0 && double.Parse(values[5], CultureInfo.InvariantCulture) == 0 && double.Parse(values[6], CultureInfo.InvariantCulture) == 0)
                            {
                                var accessoryMaterial = new AccessoryMaterial(true, false, values[0]) { Description = values[2] };
                                await accessoryMaterialRepository.InsertAsync(accessoryMaterial);

                                var accessoryMaterialFurnitureUnit = new AccessoryMaterialFurnitureUnit(furnitureUnit.Id, accessoryMaterial.Id);
                                await accessoryFurnitureUnitRepository.InsertAsync(accessoryMaterialFurnitureUnit);
                            }
                            else
                            {
                                var foilMaterial = new FoilMaterial(values[0]) { Description = values[2], Thickness = double.Parse(values[6], CultureInfo.InvariantCulture) };
                                await foilMaterialRepository.InsertAsync(foilMaterial);

                                foreach (var item in materialCodes)
                                {
                                    if (foilMaterial.Code.Contains(item))
                                    {
                                        foreach (var c in components)
                                        {
                                            if (c.BoardMaterial.Code == item)
                                            {
                                                for (int j = 0; j < foilsArrayInList.Count(); j++)
                                                {
                                                    if (i == j)
                                                    {
                                                        if (!string.IsNullOrEmpty(foilsArrayInList[j][0]))
                                                        {
                                                            c.TopFoilId = foilMaterial.Id;
                                                        }
                                                        if (!string.IsNullOrEmpty(foilsArrayInList[j][1]))
                                                        {
                                                            c.BottomFoilId = foilMaterial.Id;
                                                        }
                                                        if (!string.IsNullOrEmpty(foilsArrayInList[j][2]))
                                                        {
                                                            c.LeftFoilId = foilMaterial.Id;
                                                        }
                                                        if (!string.IsNullOrEmpty(foilsArrayInList[j][3]))
                                                        {
                                                            c.RightFoilId = foilMaterial.Id;
                                                        }
                                                    }
                                                }
                                                i++;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }

            await unitOfWork.SaveChangesAsync();

            System.IO.File.Delete(path);
        }

        public async Task<List<FurnitureUnitForDataGenerationListDto>> GetFurnitureUnitsForDataGenerationAsync()
        {
            var furnitureUnits = await furnitureUnitRepository.GetAllListAsync();
            return furnitureUnits.Select(ent => new FurnitureUnitForDataGenerationListDto(ent)).ToList();
        }

        public async Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnitsAsync(FurnitureUnitFilterDto filterDto)
        {
            Expression<Func<FurnitureUnit, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<FurnitureUnit>(FurnitureUnitFilterDto.GetOrderingMapping(), nameof(FurnitureUnit.Id));

            var furnitureUnits = await furnitureUnitRepository.GetFurnitureUnitsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return furnitureUnits.ToPagedList(FurnitureUnitListDto.FromEntity);
        }

        private static Expression<Func<FurnitureUnit, bool>> CreateFilter(FurnitureUnitFilterDto filterDto)
        {
            Expression<Func<FurnitureUnit, bool>> filter = x => true;

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

            return filter;
        }

        public async Task ParseXxlDataForCncGeneration(Guid furnitureUnitId, string containerName, string fileName)
        {
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureUnitId, ent => ent.Components);
            var fullFilePath = fileHandlerService.GetFileUrl(containerName, fileName);

            // extract to tmp directory
            var tmpDirPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", containerName, "tmp");
            ZipFile.ExtractToDirectory(fullFilePath, tmpDirPath);

            // parse each file
            string[] filePaths = Directory.GetFiles(tmpDirPath, "*.xxl");
            foreach (var filePath in filePaths)
            {
                var file = new FileInfo(filePath);
                var fileContent = System.IO.File.ReadAllText(file.FullName);
                var componentName = file.Name.Substring(0, file.Name.IndexOf("."));

                // get component and parse xxl
                var furnitureComponent = furnitureUnit.Components.Where(comp => comp.Name == componentName).FirstOrDefault();
                if(furnitureComponent != null)
                {
                    triDCorpusComponentLoader.LoadComponentFromXXLFile(ref furnitureComponent, fileContent);
                }
                else
                {
                    Directory.Delete(tmpDirPath, true);
                    throw new IFPSAppException("A component name mismatch is detected between the file and the database!");
                }
            }

            furnitureUnit.HasCncFile = true;

            // clean up tmp dir
            Directory.Delete(tmpDirPath, true);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
