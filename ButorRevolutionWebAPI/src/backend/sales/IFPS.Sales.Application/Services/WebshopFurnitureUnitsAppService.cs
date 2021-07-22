using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IFPS.Sales.Application.Services
{
    public class WebshopFurnitureUnitsAppService : ApplicationService, IWebshopFurnitureUnitsAppService
    {
        private readonly IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly IRecommendationService recommendationService;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IWebshopOrderRepository webshopOrderRepository;

        public WebshopFurnitureUnitsAppService(
            IWebshopFurnitureUnitRepository webshopFurnitureUnitRepository,
            IFileHandlerService fileHandlerService,
            IGroupingCategoryRepository groupingCategoryRepository,
            IApplicationServiceDependencyAggregate aggregate,
            IRecommendationService recommendationService,
            IFurnitureUnitRepository furnitureUnitRepository,
            IWebshopOrderRepository webshopOrderRepository) : base(aggregate)
        {
            this.webshopFurnitureUnitRepository = webshopFurnitureUnitRepository;
            this.fileHandlerService = fileHandlerService;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.recommendationService = recommendationService;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.webshopOrderRepository = webshopOrderRepository;
        }

        public async Task<int> CreateWebshopFurnitureUnitAsync(WebshopFurnitureUnitCreateDto createDto)
        {
            var wfu = createDto.CreateModelObject();

            if (createDto.Images != null)
            {
                foreach (var image in createDto.Images)
                {
                    var imageId = await fileHandlerService.InsertImage(image.ContainerName, image.FileName);
                    var wfuImage = new WebshopFurnitureUnitImage()
                    {
                        ImageId = imageId,
                        WebshopFurnitureUnitId = wfu.Id
                    };
                    wfu.AddWebshopFurnitureUnitImage(wfuImage);
                }
            }

            await webshopFurnitureUnitRepository.InsertAsync(wfu);
            await unitOfWork.SaveChangesAsync();
            return wfu.Id;
        }

        public async Task<PagedListDto<WebshopFurnitureUnitListDto>> GetWebshopFurnitureUnitsAsync(WebshopFurnitureUnitFilterDto filterDto)
        {
            Expression<Func<WebshopFurnitureUnit, bool>> filter = (WebshopFurnitureUnit ent) => ent.FurnitureUnit != null;

            if (!string.IsNullOrEmpty(filterDto.Code))
            {
                filter = filter.And(ent => ent.FurnitureUnit.Code.ToLower().Contains(filterDto.Code.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(filterDto.Description))
            {
                filter = filter.And(ent => ent.FurnitureUnit.Description.ToLower().Contains(filterDto.Description.Trim().ToLower()));
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<WebshopFurnitureUnit>(
                WebshopFurnitureUnitFilterDto.GetColumnMappings(), nameof(WebshopFurnitureUnit.Id));

            var wfus = await webshopFurnitureUnitRepository.GetWebshopFurnitureUnits(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return wfus.ToPagedList(WebshopFurnitureUnitListDto.FromEntity);
        }

        public async Task<WebshopFurnitureUnitDetailsDto> GetWebshopFurnitureUnitByIdAsync(int wfuId)
        {
            var wfu = await webshopFurnitureUnitRepository.GetWfuById(wfuId);
            return new WebshopFurnitureUnitDetailsDto(wfu);
        }

        public async Task UpdateWebshopFurnitureUnitAsync(int wfuId, WebshopFurnitureUnitUpdateDto updateDto)
        {
            var wfu = await webshopFurnitureUnitRepository.GetWfuById(wfuId);

            wfu.FurnitureUnitId = updateDto.FurnitureUnitId;
            if ((double)updateDto.Price.Value != wfu.Price.Value)
            {
                wfu.Price = updateDto.Price.CreateModelObject();
            }

            if (updateDto.Images != null)
            {
                var imagesToAdd = updateDto.Images
                    .Where(entity => !wfu.Images.Any(ent => ent.Image.ContainerName == entity.ContainerName && ent.Image.FileName == entity.FileName))
                    .ToList();
                var imagesToRemove = wfu.Images
                    .Where(entity => !updateDto.Images.Any(ent => ent.ContainerName == entity.Image.ContainerName && ent.FileName == entity.Image.FileName))
                    .ToList();

                imagesToRemove.ForEach(ent => wfu.RemoveWebshopFurnitureUnitImage(ent));

                var addedImageIds = (await Task.WhenAll(
                    imagesToAdd.Select(x =>
                         fileHandlerService.InsertImage(x.ContainerName, x.FileName)
                    )));
                addedImageIds.Select(x => new WebshopFurnitureUnitImage()
                    {
                        WebshopFurnitureUnitId = wfuId,
                        ImageId = x,
                    })
                    .ToList()
                    .ForEach(ent => wfu.AddWebshopFurnitureUnitImage(ent));
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteWebshopFurnitureUnitAsync(int wfuId)
        {
            await webshopFurnitureUnitRepository.DeleteAsync(wfuId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> GetWebshopFurnitureUnitsByWebshopCategoryAsync(int subcategoryId, WebshopFurnitureUnitFilterByWebshopCategoryDto filterDto)
        {
            Expression<Func<WebshopFurnitureUnit, bool>> filter = (WebshopFurnitureUnit c) => c.FurnitureUnit.Code != null;

            if (filterDto.UnitType != FurnitureUnitTypeEnum.None)
            {
                if (filterDto.UnitType == FurnitureUnitTypeEnum.Top)
                {
                    filter = filter.And(ent => ent.FurnitureUnit.FurnitureUnitType.Type == filterDto.UnitType);
                }
                else
                {
                    filter = filter.And(ent => ent.FurnitureUnit.FurnitureUnitType.Type != filterDto.UnitType);
                }
            }

            if (filterDto.MinimumPrice != 0 && filterDto.MaximumPrice != 0)
            {
                filter = filter.And(ent => ent.Price.Value >= filterDto.MinimumPrice && ent.Price.Value <= filterDto.MaximumPrice);
            }

            var category = await groupingCategoryRepository.SingleAsync(ent => ent.Id == subcategoryId);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<WebshopFurnitureUnit>(WebshopFurnitureUnitFilterByWebshopCategoryDto.GetOrderingMapping(), nameof(FurnitureUnit.Id));

            var webshopFurnitureUnits = await webshopFurnitureUnitRepository.GetWebshopFurnitureUnitsByCategoryTypeAsync(category.CategoryType, filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return webshopFurnitureUnits.ToPagedList(WebshopFurnitureUnitListByWebshopCategoryDto.FromEntity);
        }

        public async Task<WebshopFurnitureUnitByWebshopDetailsDto> GetWebshopFurnitureUnitByWebshopByIdAsync(int webshopFurnitureUnitId)
        {
            var wfu = await webshopFurnitureUnitRepository.GetWebshopFurnitureUnitByWebshopByIdAsync(webshopFurnitureUnitId);
            return new WebshopFurnitureUnitByWebshopDetailsDto(wfu);
        }

        public async Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> SearchWebshopFurnitureUnitInWebshopAsync([FromQuery] WebshopFurnitureUnitFilterDto filterDto)
        {
            Expression<Func<WebshopFurnitureUnit, bool>> filter = (WebshopFurnitureUnit ent) => true;
            if (!string.IsNullOrEmpty(filterDto.Code))
            {
                filter = filter.And(ent => ent.FurnitureUnit.Code.ToLower().Trim().Contains(filterDto.Code.ToLower().Trim()));
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<WebshopFurnitureUnit>(WebshopFurnitureUnitFilterByWebshopCategoryDto.GetOrderingMapping(), nameof(FurnitureUnit.Id));

            var webshopFurnitureUnits = await webshopFurnitureUnitRepository.GetWebshopFurnitureUnitsBySearch(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return webshopFurnitureUnits.ToPagedList(WebshopFurnitureUnitListByWebshopCategoryDto.FromEntity);
        }

        public async Task<PriceListDto> GetMaximumPriceFromWFUListAsync(WebshopFurnitureUnitCategoryIdsDto dto)
        {
            var wfus = new List<WebshopFurnitureUnit>();
            foreach (var categoryId in dto.CategoryIds)
            {
                var wfuList = await webshopFurnitureUnitRepository.GetAllListIncludingAsync(ent => ent.FurnitureUnit.CategoryId.Value == categoryId, ent => ent.Price.Currency);
                if (wfuList != null)
                {
                    wfus.AddRange(wfuList);
                }
            }

            var price = Price.GetDefaultPrice();
            price.Currency = Currency.GetDeafultCurrency();
            var result = new PriceListDto(price);
            if (wfus.Count() > 0)
            {
                var wfu = wfus.OrderByDescending(ent => ent.Price.Value).First();
                result.Value = wfu.Price.Value;
                result.Currency = wfu.Price.Currency.Name;
            }
            return result;
        }

        public Task<List<WebshopFurnitureUnitDropdownListDto>> GetWebshopFurnitureUnitsForDropdownAsync()
        {
            return webshopFurnitureUnitRepository.GetAllListAsync(ent => true, WebshopFurnitureUnitDropdownListDto.Projection);
        }

        public async Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetRecommendedFurnitureUnitsAsync(WebshopFurnitureUnitInBasketIdsDto dto)
        {
            /// quick fix - static FU codes (frontend modification required to make it work as intended)
            var staticFuCodes = new List<string> { 
                "D5L",
                "D2L",
                "D2V",
                "D1V"
            };

            Random rng = new Random();
            int idx = rng.Next(0, 4);
            var selectedFuCode = staticFuCodes[idx];
            var selectedFu = await furnitureUnitRepository.SingleAsync(ent => ent.Code == selectedFuCode);

            dto.FurnitureUnitIds = new List<Guid> { selectedFu.Id };

            /// quick fix end, delete this if properly connected

            // always sort FU codes before usage!
            var allfurnitureUnitCodes = furnitureUnitRepository.GetAllList().Select(fu => fu.Code).OrderBy(x => x).ToList();
            var furnitureUnitsInBasket = furnitureUnitRepository.GetAllList().Where(ent => dto.FurnitureUnitIds.Contains(ent.Id)).ToList();

            //TODO: filename and container to config?
            var rulesFileName = fileHandlerService.GetFileFullPath("CartAnalysis", "cart-rules.json");
            string jsonRules = System.IO.File.ReadAllText(rulesFileName);
            var ruleDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(jsonRules);

            var fuCodes = await recommendationService.GetRecommendedFurnitureUnits(furnitureUnitsInBasket, allfurnitureUnitCodes, ruleDict, dto.RecommendedItemNum);

            var wfus = await webshopFurnitureUnitRepository
                .GetAllListIncludingAsync(ent => fuCodes.Contains(ent.FurnitureUnit.Code), ent => ent.Price.Currency, ent => ent.FurnitureUnit.Image, ent => ent.Images);
            var result = wfus.Select(x => new WebshopFurnitureUnitListByWebshopCategoryDto(x)).ToList();

            return result;
        }

        public async Task<(string ContainerName, string Filename)> UpdateRecommendationRulesAsync()
        {
            // always sort FU codes before usage!
            var furnitureUnitCodes = furnitureUnitRepository.GetAllList().Select(fu => fu.Code).OrderBy(x => x).ToList();            
            var webshopOrdersAsFuCodeLists = await webshopOrderRepository.GetOrderedFUCodes();

            // create bool lists for the algorithm (https://github.com/cotur/DataMining)
            var basketsAsBoolLists = new List<bool[]>();
            foreach (var webshopOrderCodes in webshopOrdersAsFuCodeLists)
            {
                var tmp = new bool[furnitureUnitCodes.Count];
                for (int i = 0; i < furnitureUnitCodes.Count; i++)
                {
                    if(webshopOrderCodes.Contains(furnitureUnitCodes[i]))
                    {
                        tmp[i] = true;
                    }
                }
                basketsAsBoolLists.Add(tmp);
            }      

            var ruleDict = recommendationService.GenerateRuleSet(furnitureUnitCodes, basketsAsBoolLists);

            var(containerName, fileName) = await fileHandlerService.UploadJsonFileAsync(ruleDict, "CartAnalysis", "cart-rules.json");

            return (containerName, fileName);
        }
    }
}
