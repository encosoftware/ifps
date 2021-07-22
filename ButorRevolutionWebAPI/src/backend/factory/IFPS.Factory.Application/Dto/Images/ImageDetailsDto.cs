using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ImageDetailsDto
    {
        public string ContainerName { get; set; }
        public string FileName { get; set; }

        public ImageDetailsDto(Image image)
        {
            ContainerName = image.ContainerName;
            FileName = image.FileName;
        }
    }
}