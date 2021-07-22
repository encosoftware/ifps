using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ISortingStrategyTypesAppService
    {
        Task<List<SortingStrategyTypeListDto>> GetSortingStrategyTypesAsync();
    }
}
