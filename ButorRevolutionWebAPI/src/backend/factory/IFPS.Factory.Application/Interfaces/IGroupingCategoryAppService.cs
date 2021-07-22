using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IGroupingCategoryAppService
    {
        Task<List<GroupingCategoryListDto>> GetFlatGroupingCategoriesAsync(GroupingCategoryFilterDto filter);
        Task<List<GroupingCategoryListDto>> GetHierarchicalGroupingCategoriesAsync(GroupingCategoryFilterDto filter);
        Task<GroupingCategoryDetailsDto> GetGroupingCategoryDetailsAsync(int id);
    }
}
