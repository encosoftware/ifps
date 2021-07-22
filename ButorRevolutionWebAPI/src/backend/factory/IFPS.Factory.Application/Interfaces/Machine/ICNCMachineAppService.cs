using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICncMachineAppService
    {
        Task<PagedListDto<CncMachineListDto>> GetCncMachinesAsync(CncMachineFilterDto dto);
        Task<CncMachineDetailsDto> GetCncMachineByIdAsync(int id);
        Task UpdateCncMachineAsync(int id, CncMachineUpdateDto dto);
        Task<int> CreateCncMachineAsync(CncMachineCreateDto dto);
        Task DeleteCncMachineAsync(int id);
    }
}
