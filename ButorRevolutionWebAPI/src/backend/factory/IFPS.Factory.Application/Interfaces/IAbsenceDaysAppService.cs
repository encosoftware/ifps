using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IAbsenceDaysAppService
    {
        Task<List<AbsenceDayDto>> GetAbsenceDaysAsync(int userId, int year, int month);
        Task AddOrUpdateAbsenceDaysAsync(int userId, List<AbsenceDayDto> dtos);
        Task DeleteAbsenceDaysAsync(int userId, AbsenceDaysDeleteDto absenceDays);
    }
}
