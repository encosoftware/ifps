using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class WorkStationCamera : FullAuditedAggregateRoot
    {
        public virtual Camera Camera { get; set; }
        public int? CameraId { get; set; }

        public WorkStation WorkStation { get; set; }
        public int WorkStationId { get; set; }

        public CFCProductionState CFCProductionState { get; set; }
        public int CFCProductionStateId { get; set; }

        private WorkStationCamera() { }

        public WorkStationCamera(int cameraId, int workStationId, int cfcProductionStateId)
        {
            CameraId = cameraId;
            WorkStationId = workStationId;
            CFCProductionStateId = cfcProductionStateId;
        }
    }
}
