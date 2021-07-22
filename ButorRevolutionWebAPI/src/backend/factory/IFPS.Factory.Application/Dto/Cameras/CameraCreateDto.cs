using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CameraCreateDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }

        public CameraCreateDto() { }

        public Camera CreateModelObject()
        {
            return new Camera(Name, Type, IPAddress);
        }
    }
}
