using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class WorkStation : FullAuditedAggregateRoot
    {
        public string Name { get; set; }

        public int OptimalCrew { get; set; }

        public int SecPerComponent { get; set; }

        public virtual Machine Machine { get; set; }
        public int? MachineId { get; set; }

        public WorkStationType WorkStationType { get; set; }
        public int WorkStationTypeId { get; set; }


        private List<Plan> _plans;
        public IEnumerable<Plan> Plans => _plans.AsReadOnly();

        /// <summary>
        /// List of cameras on the workstation.
        /// </summary>
        private List<WorkStationCamera> _workStationCameras;
        public IEnumerable<WorkStationCamera> WorkStationCameras => _workStationCameras.AsReadOnly();

        public bool IsActive { get; set; }

        private WorkStation()
        {
            _workStationCameras = new List<WorkStationCamera>();
            _plans = new List<Plan>();
        }

        public WorkStation(string name, int optimalCrew, bool isActive, int workStationTypeId) : this()
        {
            Name = name;
            OptimalCrew = optimalCrew;
            IsActive = isActive;
            WorkStationTypeId = workStationTypeId;
        }

        public void AddWorkStationCamera(WorkStationCamera workStationCamera)
        {
            Ensure.NotNull(workStationCamera);
            _workStationCameras.Add(workStationCamera);
        }

        public void RemoveWorkStationCamera(WorkStationCamera workStationCamera)
        {
            Ensure.NotNull(workStationCamera);
            _workStationCameras.Remove(workStationCamera);
        }
    }
}
