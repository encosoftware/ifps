
using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IWorkStationsAppService
    {
        Task<PagedListDto<WorkStationListDto>> GetWorkStationsAsync(WorkStationFilterDto filterDto);
        Task<WorkStationDetailsDto> GetWorkStationAsync(int workStationId);
        Task<int> CreateWorkStationAsync(WorkStationCreateDto createDto);
        Task UpdateWorkStationAsync(int workStationId, WorkStationUpdateDto updateDto);
        Task DeleteWorkStationAsync(int workStationId);
        Task SetAvailabilityOfWorkStationAsync(int workStationId);
        Task AddCamerasAsync(int workStationId, WorkStationCameraCreateDto workStationCameraCreateDto);
        Task<WorkStationCameraDetailsDto> GetCamerasAsync(int workStationId);
        Task<WorkStationsPlansListDto> GetWorkStationsByWorkloadPageAsync();
    }
}
