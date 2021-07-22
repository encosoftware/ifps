using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CameraNameListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CameraNameListDto(Camera camera)
        {
            Id = camera.Id;
            Name = camera.Name;
        }
    }
}
