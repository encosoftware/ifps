using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IDayTypeAppService
    {
        Task<List<DayTypeListDto>> GetDayTypesAsync();
        Task<List<DayTypeListDto>> GetWeekDaysAsync();
    }
}
