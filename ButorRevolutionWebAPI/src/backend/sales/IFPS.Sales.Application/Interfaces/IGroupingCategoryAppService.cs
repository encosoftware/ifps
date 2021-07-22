using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IGroupingCategoryAppService
    {
        Task<List<GroupingCategoryListDto>> GetFlatGroupingCategoriesAsync(GroupingCategoryFilterDto filter);
        Task<List<GroupingCategoryListDto>> GetHierarchicalGroupingCategoriesAsync(GroupingCategoryFilterDto filter);
        Task<List<GroupingCategoryWebshopListDto>> GetFurnitureUnitGroupingCategoriesForWebshopAsync();
        Task<List<GroupingSubCategoryWebshopListDto>> GetSubcategeroiesByCategoryAsync(int categoryId);
        Task<GroupingCategoryDetailsDto> GetGroupingCategoryDetailsAsync(int id);
        Task<List<GroupingCategoryListDto>> GetCategoriesForDropdownAsync(GroupingCategoryEnum filter);
        Task<int> CreateGroupingCategoryAsync(GroupingCategoryCreateDto userDto);
        Task UpdateGroupingCategoryAsync(int id, GroupingCategoryUpdateDto userDto);
        Task DeleteGroupingCategoryAsync(int id);
        Task<List<DecorBoardGroupingCategoryListDto>> GetDecorBoardCategoriesAsync();
    }
}
