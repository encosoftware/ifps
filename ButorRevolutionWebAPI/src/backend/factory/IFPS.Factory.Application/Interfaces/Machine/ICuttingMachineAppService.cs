using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICuttingMachineAppService
    {
        Task<PagedListDto<CuttingMachineListDto>> GetCuttingMachinesAsync(CuttingMachineFilterDto dto);
        Task<CuttingMachineDetailsDto> GetCuttingMachineByIdAsync(int id);
        Task UpdateCuttingMachineAsync(int id, CuttingMachineUpdateDto dto);
        Task<int> CreateCuttingMachineAsync(CuttingMachineCreateDto dto);
        Task DeleteCuttingMachineAsync(int id);
    }
}
