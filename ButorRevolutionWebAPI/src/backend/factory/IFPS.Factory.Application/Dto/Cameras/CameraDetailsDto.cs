using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CameraDetailsDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }

        public CameraDetailsDto(Camera camera)
        {
            Name = camera.Name;
            Type = camera.Type;
            IPAddress = camera.IPAddress;
        }
    }
}
