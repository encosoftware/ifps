using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICameraAppService
    {
        Task<int> CreateCameraAsync(CameraCreateDto cameraCreateDto);
        Task<PagedListDto<CameraListDto>> GetCamerasAsync(CameraFilterDto cameraFilterDto);
        Task<CameraDetailsDto> GetCameraAsync(int id);
        Task UpdateCameraAsync(int id, CameraUpdateDto cameraUpdateDto);
        Task DeleteCameraAsync(int id);
        Task<List<CameraNameListDto>> GetCameraNameListAsync(int workStationId);
    }
}
