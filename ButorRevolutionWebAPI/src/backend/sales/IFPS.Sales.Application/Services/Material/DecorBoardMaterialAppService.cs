using System;
using System.Collections.Generic;
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
    public class DecorBoardMaterialAppService : ApplicationService, IDecorBoardMaterialAppService
    {
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;

        public DecorBoardMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IDecorBoardMaterialRepository decorBoardMaterialRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ICurrencyRepository currencyRepository,
            ILanguageRepository languageRepository,
            IFileHandlerService fileHandlerService
            )
            : base(aggregate)
        {
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
        }

        public async Task<Guid> CreateDecorBoardMaterialAsync(DecorBoardMaterialCreateDto decorBoardMaterialCreateDto)
        {
            var decorBoardMaterial = decorBoardMaterialCreateDto.CreateModelObject();
            decorBoardMaterial.AddPrice(new MaterialPrice() { Price = decorBoardMaterialCreateDto.Price.CreateModelObject() });
            decorBoardMaterial.ImageId = await fileHandlerService.InsertImage(decorBoardMaterialCreateDto.ImageCreateDto.ContainerName, decorBoardMaterialCreateDto.ImageCreateDto.FileName);

            await decorBoardMaterialRepository.InsertAsync(decorBoardMaterial);
            await unitOfWork.SaveChangesAsync();

            return decorBoardMaterial.Id;
        }

        public async Task DeleteMaterialAsync(Guid id)
        {
            await decorBoardMaterialRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<DecorBoardMaterialDetailsDto> GetDecorBoardMaterialAsync(Guid id)
        {
            return new DecorBoardMaterialDetailsDto(await decorBoardMaterialRepository
                .SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category));
        }

        public async Task<List<DecorBoardMaterialCodesDto>> GetDecorBoardMaterialCodesAsync()
        {
            return await decorBoardMaterialRepository.GetAllListAsync(ent => ent.Code != null, DecorBoardMaterialCodesDto.Projection);
        }

        public async Task<List<DecorBoardMaterialWithImageDto>> GetDecorBoardMaterialForDropdownAsync()
        {
            return await decorBoardMaterialRepository.GetAllListAsync(ent => ent.Code != null, DecorBoardMaterialWithImageDto.Projection);
        }

        public async Task<PagedListDto<DecorBoardMaterialListDto>> GetDecorBoardMaterialsAsync(DecorBoardMaterialFilterDto filterDto)
        {
            Expression<Func<DecorBoardMaterial, bool>> filter = (DecorBoardMaterial c) => c.Code != null;

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

                if (filterDto.CategoryId != 0)
                {
                    filter = filter.And(ent => ent.CategoryId.Equals(filterDto.CategoryId));
                }
            }

            var columnMappings = new Dictionary<string, string>
            {
                { nameof(filterDto.Code), nameof(DecorBoardMaterial.Code) },
                { nameof(filterDto.Description), nameof(DecorBoardMaterial.Description) },
                { nameof(filterDto.CategoryId), nameof(DecorBoardMaterial.CategoryId) }
            };
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<DecorBoardMaterial>(columnMappings, nameof(DecorBoardMaterial.Id));

            var decorboardMaterials = await decorBoardMaterialRepository.GetDecorBoardMaterialsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return decorboardMaterials.ToPagedList(DecorBoardMaterialListDto.FromEntity);
        }

        public async Task UpdateDecorBoardMaterialAsync(Guid id, DecorBoardMaterialUpdateDto decorBoardMaterialUpdateDto)
        {
            var decorBoardMaterial = await decorBoardMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            decorBoardMaterial = decorBoardMaterialUpdateDto.UpdateModelObject(decorBoardMaterial);
            decorBoardMaterial.Image = await fileHandlerService.UpdateImage(decorBoardMaterial.ImageId.Value, decorBoardMaterialUpdateDto.ImageUpdateDto.ContainerName, decorBoardMaterialUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
