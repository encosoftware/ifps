using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationCameraDetailsDto
    {
        public WorkStationCameraListDto Start { get; set; }
        public WorkStationCameraListDto Finish { get; set; }

        public WorkStationCameraDetailsDto(WorkStation workStation)
        {
            var started = workStation.WorkStationCameras.SingleOrDefault(ent => ent.CFCProductionState.State == CFCProductionStateEnum.Started);
            var finished = workStation.WorkStationCameras.SingleOrDefault(ent => ent.CFCProductionState.State == CFCProductionStateEnum.Finished);
            if (started != null)
            {
                Start = new WorkStationCameraListDto(started);
            }
            if (finished != null)
            {
                Finish = new WorkStationCameraListDto(finished);
            }
        }
    }
}
