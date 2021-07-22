using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IEdgingMachineAppService
    {
        Task<PagedListDto<EdgingMachineListDto>> GetEdgingMachinesAsync(EdgingMachineFilterDto dto);
        Task<EdgingMachineDetailsDto> GetEdgingMachineByIdAsync(int id);
        Task UpdateEdgingMachineAsync(int id, EdgingMachineUpdateDto dto);
        Task<int> CreateEdgingMachineAsync(EdgingMachineCreateDto dto);
        Task DeleteEdgingMachineAsync(int id);
    }
}
