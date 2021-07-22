using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CameraUpdateDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }

        public Camera UpdateModelObject(Camera camera)
        {
            camera.Name = Name;
            camera.Type = Type;
            camera.IPAddress = IPAddress;
            return camera;
        }
    }
}
