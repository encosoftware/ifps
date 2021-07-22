using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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

        public async Task<List<GroupingCategoryListDto>> GetHierarchicalGroupingCategoriesAsync(GroupingCategoryFilterDto filterDto)
        {
            Expression<Func<GroupingCategory, bool>> filter;

            if (filterDto.CategoryType == Domain.Enums.GroupingCategoryEnum.None)
            {
                filter = (GroupingCategory t) => t.CategoryType != filterDto.CategoryType;
            }
            else
            {
                filter = (GroupingCategory t) => t.CategoryType == filterDto.CategoryType;
            }

            return (await groupingCategoryRepository.GetAllListIncludingAsync(filter))
                .Where(ent => !ent.ParentGroupId.HasValue)
                .Select(ent => new GroupingCategoryListDto(ent, true)).ToList();
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
    }
}
