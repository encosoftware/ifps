using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
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
