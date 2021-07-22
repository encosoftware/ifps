using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class Camera : FullAuditedAggregateRoot
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }
        public virtual WorkStationCamera WorkStationCamera { get; set; }
        public int? WorkStationCameraId { get; set; }
        public bool IsActive { get; set; }

        public Camera() { }

        public Camera(string name, string type, string ipAddress)
        {
            Name = name;
            Type = type;
            IPAddress = ipAddress;
        }
    }
}
