using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CameraTestSeed : IEntitySeed<Camera>
    {
        public Camera[] Entities = new Camera[]
        {
            new Camera("Test camera", "Action", "192.168.0.1") { Id = 10000, WorkStationCameraId = 10000 },
            new Camera("Test Axis", "Advanture", "192.168.10.1") { Id = 10001, WorkStationCameraId = 10001 },
            new Camera("Test Nikon", "Action", "192.150.0.10") { Id = 10002, WorkStationCameraId = 10002 },
            new Camera("Test Canon", "Action", "192.155.0.1") { Id = 10003, WorkStationCameraId = 10003 },
            new Camera("Test camera 2", "Still", "192.150.0.1") { Id = 10004, WorkStationCameraId = 10004 },
            new Camera("Test camera 3", "Movement", "192.168.120.1") { Id = 10005, WorkStationCameraId = 10005 },
            new Camera("Test camera 4", "Action", "192.168.15.1") { Id = 10006, WorkStationCameraId = 10006 },
        };
    }
}
