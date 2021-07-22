using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Exceptions;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class GroupingCategoryAppService : ApplicationService, IGroupingCategoryAppService
    {
        private IGroupingCategoryRepository groupingCategoryRepository;

        public GroupingCategoryAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IGroupingCategoryRepository groupingCategoryRepository
           ) : base(aggregate)
        {
            this.groupingCategoryRepository = groupingCategoryRepository;
        }

        public async Task<int> CreateGroupingCategoryAsync(GroupingCategoryCreateDto createDto)
        {
            var validationBuilder = new ValidationExceptionBuilder<GroupingCategoryCreateDto>();
            var parent = groupingCategoryRepository.SingleOrDefault(x => x.Id == createDto.ParentId);
            if (parent == null)
            {
                validationBuilder.AddError(x => x.ParentId, $"Invalid {nameof(createDto.ParentId)}");
            }

            validationBuilder.ThrowIfHasError();

            var newCategory = createDto.ConvertToModelObject(parent);

            await groupingCategoryRepository.InsertAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            return newCategory.Id;
        }

        public async Task<List<GroupingCategoryListDto>> GetHierarchicalGroupingCategoriesAsync(GroupingCategoryFilterDto filterDto)
        {
            Expression<Func<GroupingCategory, bool>> filter;

            if (filterDto.CategoryType == GroupingCategoryEnum.None)
            {
                filter = (GroupingCategory t) => t.CategoryType != filterDto.CategoryType;
            }
            else
            {
                filter = (GroupingCategory t) => t.CategoryType == filterDto.CategoryType;
            }

            var cats = (await groupingCategoryRepository.GetAllListIncludingAsync(filter))
                .Where(ent => !ent.ParentGroupId.HasValue)
                .Select(ent => new GroupingCategoryListDto(ent, true)).ToList();
            return cats;
        }

        public async Task<List<GroupingCategoryListDto>> GetFlatGroupingCategoriesAsync(GroupingCategoryFilterDto filterDto)
        {
            Expression<Func<GroupingCategory, bool>> filter;

            if (filterDto.CategoryType == GroupingCategoryEnum.None)
            {
                filter = (GroupingCategory t) => t.CategoryType != filterDto.CategoryType;
            }
            else
            {
                filter = (GroupingCategory t) => t.CategoryType == filterDto.CategoryType;
            }

            if (filterDto.LoadOnlyRootObjects)
            {
                filter = filter.And(e => !e.ParentGroupId.HasValue);
            }

            return (await groupingCategoryRepository.GetAllListIncludingAsync(filter))
                .Select(ent => new GroupingCategoryListDto(ent, false)).ToList();
        }

        public async Task<GroupingCategoryDetailsDto> GetGroupingCategoryDetailsAsync(int id)
        {
            return new GroupingCategoryDetailsDto(await groupingCategoryRepository
                .SingleIncludingAsync(ent => ent.Id == id, ent => ent.Children, ent => ent.Translations));
        }

        public async Task UpdateGroupingCategoryAsync(int id, GroupingCategoryUpdateDto updateDto)
        {
            var validationBuilder = new ValidationExceptionBuilder<GroupingCategoryUpdateDto>();

            var category = groupingCategoryRepository.SingleIncluding(x => x.Id == id, ent => ent.Translations);
            foreach (var translation in updateDto.Translations)
            {
                if (!category.Translations.Any(ent => ent.Id == translation.Id))
                {
                    validationBuilder.AddError(x => x.Translations, $"Invalid {updateDto.Translations}.Id");
                }
            }

            validationBuilder.ThrowIfHasError();

            foreach (var newTranslation in updateDto.Translations)
            {
                category.Translations.Single(ent => ent.Id == newTranslation.Id).GroupingCategoryName = newTranslation.Name;
            }

            category.ParentGroupId = updateDto.ParentGroupId;
            await groupingCategoryRepository.UpdateAsync(category);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteGroupingCategoryAsync(int id)
        {
            var validationBuilder = new ValidationExceptionBuilder<GroupingCategory>();

            var groupingCategory = await groupingCategoryRepository
                .SingleIncludingAsync(ent => ent.Id == id, ent => ent.Children, ent => ent.Translations);

            if (groupingCategory.Children.Count > 0)
            {
                validationBuilder.AddError(x => x.Children, $"Cant delete grouping category with valid children");
            }

            validationBuilder.ThrowIfHasError();

            groupingCategory.IsDeleted = true;

            await groupingCategoryRepository.UpdateAsync(groupingCategory);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GroupingCategoryWebshopListDto>> GetFurnitureUnitGroupingCategoriesForWebshopAsync()
        {
            var webshopCategories = await groupingCategoryRepository.GetWebshopCategoriesByParentEnumTypeAsync(GroupingCategoryEnum.FurnitureUnitType);
            return webshopCategories.Select(ent => new GroupingCategoryWebshopListDto(ent)).ToList();
        }

        public async Task<List<GroupingSubCategoryWebshopListDto>> GetSubcategeroiesByCategoryAsync(int categoryId)
        {
            var parent = await groupingCategoryRepository.SingleAsync(ent => ent.Id == categoryId);
            var subcategories = await groupingCategoryRepository.GetWebshopCategoriesByParentEnumTypeAsync(parent.CategoryType);
            return subcategories.Select(ent => new GroupingSubCategoryWebshopListDto(ent)).ToList();
        }

        public async Task<List<GroupingCategoryListDto>> GetCategoriesForDropdownAsync(GroupingCategoryEnum type)
        {
            var categories = await groupingCategoryRepository.GetAllListForDropdownAsync(type);
            return categories.Select(x => new GroupingCategoryListDto(x)).ToList();
        }

        public async Task<List<DecorBoardGroupingCategoryListDto>> GetDecorBoardCategoriesAsync()
        {
            var decorBoardsCategories = await groupingCategoryRepository.GetAllDecorBoardCategoryAsync();
            return decorBoardsCategories.Select(ent => new DecorBoardGroupingCategoryListDto(ent)).ToList();
        }
    }
}
