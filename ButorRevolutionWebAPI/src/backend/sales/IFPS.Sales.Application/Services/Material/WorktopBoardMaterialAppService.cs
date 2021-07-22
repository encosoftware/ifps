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
    public class WorktopBoardMaterialAppService : ApplicationService, IWorktopBoardMaterialAppService
    {
        private readonly IWorktopBoardMaterialRepository worktopBoardMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly ILanguageRepository languageRepository;

        public WorktopBoardMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IWorktopBoardMaterialRepository worktopBoardMaterialRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            ICurrencyRepository currencyRepository,
            ILanguageRepository languageRepository,
            IFileHandlerService fileHandlerService
            )
            : base(aggregate)
        {
            this.worktopBoardMaterialRepository = worktopBoardMaterialRepository;
            this.fileHandlerService = fileHandlerService;
            this.currencyRepository = currencyRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.languageRepository = languageRepository;
        }

        public async Task<Guid> CreateWorktopBoardMaterialAsync(WorktopBoardMaterialCreateDto worktopBoardMaterialCreateDto)
        {
            var worktopBoardMaterial = worktopBoardMaterialCreateDto.CreateModelObject();
            worktopBoardMaterial.AddPrice(new MaterialPrice() { Price = worktopBoardMaterialCreateDto.Price.CreateModelObject() });
            worktopBoardMaterial.ImageId = await fileHandlerService.InsertImage(worktopBoardMaterialCreateDto.ImageCreateDto.ContainerName, worktopBoardMaterialCreateDto.ImageCreateDto.FileName);

            await worktopBoardMaterialRepository.InsertAsync(worktopBoardMaterial);
            await unitOfWork.SaveChangesAsync();

            return worktopBoardMaterial.Id;
        }

        public async Task DeleteMaterialAsync(Guid id)
        {
            await worktopBoardMaterialRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<WorktopBoardMaterialDetailsDto> GetWorktopBoardMaterialAsync(Guid id)
        {
            var worktopBoardMaterial = await worktopBoardMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            return new WorktopBoardMaterialDetailsDto(worktopBoardMaterial);
        }

        public async Task<PagedListDto<WorktopBoardMaterialListDto>> GetWorktopBoardMaterialsAsync(WorktopBoardMaterialFilterDto filterDto)
        {
            Expression<Func<WorktopBoardMaterial, bool>> filter = (WorktopBoardMaterial c) => c.Code != null;

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
                { nameof(filterDto.Code), nameof(WorktopBoardMaterial.Code) },
                { nameof(filterDto.Description), nameof(WorktopBoardMaterial.Description) },
                { nameof(filterDto.CategoryId), nameof(WorktopBoardMaterial.CategoryId) }
            };
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<WorktopBoardMaterial>(columnMappings, nameof(WorktopBoardMaterial.Id));

            var decorboardMaterials = await worktopBoardMaterialRepository.GetWorktopBoardMaterialsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return decorboardMaterials.ToPagedList(WorktopBoardMaterialListDto.FromEntity);
        }

        public async Task UpdateWorktopBoardMaterialAsync(Guid id, WorktopBoardMaterialUpdateDto worktopBoardMaterialUpdateDto)
        {
            var worktopBoardMaterial = await worktopBoardMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            worktopBoardMaterial = worktopBoardMaterialUpdateDto.UpdateModelObject(worktopBoardMaterial);
            worktopBoardMaterial.Image = await fileHandlerService.UpdateImage(worktopBoardMaterial.ImageId.Value, worktopBoardMaterialUpdateDto.ImageUpdateDto.ContainerName, worktopBoardMaterialUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
