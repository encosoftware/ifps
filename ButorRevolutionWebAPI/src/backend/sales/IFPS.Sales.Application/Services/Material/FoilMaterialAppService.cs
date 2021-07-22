using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using LinqKit;

namespace IFPS.Sales.Application.Services
{
    public class FoilMaterialAppService : ApplicationService, IFoilMaterialAppService
    {
        private readonly IFoilMaterialRepository foilMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;

        public FoilMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IFoilMaterialRepository foilMaterialRepository,
            IFileHandlerService fileHandlerService,
            ICurrencyRepository currencyRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ILanguageRepository languageRepository
            )
            : base(aggregate)
        {
            this.foilMaterialRepository = foilMaterialRepository;
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
        }

        public async Task<Guid> CreateFoilMaterialAsync(FoilMaterialCreateDto foilMaterialCreateDto)
        {
            var foilMaterial = foilMaterialCreateDto.CreateModelObject();
            foilMaterial.AddPrice(new MaterialPrice() { Price = foilMaterialCreateDto.Price.CreateModelObject() });
            foilMaterial.ImageId = await fileHandlerService.InsertImage(foilMaterialCreateDto.ImageCreateDto.ContainerName, foilMaterialCreateDto.ImageCreateDto.FileName);

            await foilMaterialRepository.InsertAsync(foilMaterial);
            await unitOfWork.SaveChangesAsync();

            return foilMaterial.Id;
        }

        public async Task DeleteMaterialAsync(Guid id)
        {
            await foilMaterialRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<FoilMaterialDetailsDto> GetFoilMaterialAsync(Guid id)
        {
            var foilMaterial = await foilMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image);
            return new FoilMaterialDetailsDto(foilMaterial);
        }

        public async Task<PagedListDto<FoilMaterialListDto>> GetFoilMaterialsAsync(FoilMaterialFilterDto filterDto)
        {
            Expression<Func<FoilMaterial, bool>> filter = (FoilMaterial c) => c.Code != null;

            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.Code.ToLower().Contains(filterDto.Code.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Description))
                {
                    filter = filter.And(ent => ent.Description.ToLower().Contains(filterDto.Description.ToLower()));
                }
            }

            var columnMappings = new Dictionary<string, string>
            {
                { nameof(filterDto.Code), nameof(FoilMaterial.Code) },
                { nameof(filterDto.Description), nameof(FoilMaterial.Description) }
            };
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<FoilMaterial>(columnMappings, nameof(FoilMaterial.Id));

            var foilMaterials = await foilMaterialRepository.GetFoilMaterialsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return foilMaterials.ToPagedList(FoilMaterialListDto.FromEntity);
        }

        public async Task UpdateFoilMaterialAsync(Guid id, FoilMaterialUpdateDto foilMaterialUpdateDto)
        {
            var foilMaterial = await foilMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image);
            foilMaterial = foilMaterialUpdateDto.UpdateModelObject(foilMaterial);
            foilMaterial.Image = await fileHandlerService.UpdateImage(foilMaterial.ImageId.Value, foilMaterialUpdateDto.ImageUpdateDto.ContainerName, foilMaterialUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FoilsForDropdownDto>> GetFoilsForDropdownAsync()
        {
            var foils = await foilMaterialRepository.GetAllListAsync();
            return foils.Select(ent => new FoilsForDropdownDto(ent)).ToList();
        }
    }
}
