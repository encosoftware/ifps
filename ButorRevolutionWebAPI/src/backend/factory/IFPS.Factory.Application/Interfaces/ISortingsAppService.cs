using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ISortingsAppService
    {
        Task<PagedListDto<SortingListDto>> GetSortingsAsync(SortingFilterDto filterDto);
    }
}
