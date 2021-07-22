using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ImageThumbnailDetailsDto
    {
        public string ContainerName { get; set; }
        public string FileName { get; set; }

        public ImageThumbnailDetailsDto(Image image)
        {
            ContainerName = image.ContainerName;
            FileName = image.ThumbnailName;
        }
    }
}