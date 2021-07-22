using IFPS.Sales.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IAbsenceDaysAppService
    {
        Task<List<AbsenceDayDto>> GetAbsenceDaysAsync(int userId, int year, int month);
        Task AddOrUpdateAbsenceDaysAsync(int userId, List<AbsenceDayDto> dtos);
        Task DeleteAbsenceDaysAsync(int userId, AbsenceDaysDeleteDto absenceDays);
    }
}
