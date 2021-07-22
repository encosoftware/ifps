using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationCameraListDto
    {
        public int? CFCProductionStateId { get; set; }
        public int? CameraId { get; set; }

        public WorkStationCameraListDto() { }

        public WorkStationCameraListDto(WorkStationCamera workStationCamera)
        {
            CFCProductionStateId = workStationCamera.CFCProductionStateId;
            CameraId = workStationCamera.CameraId;
        }
    }
}
