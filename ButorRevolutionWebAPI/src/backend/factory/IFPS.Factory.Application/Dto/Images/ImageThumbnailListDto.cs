using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ImageThumbnailListDto
    {
        public string ContainerName { get; set; }
        public string FileName { get; set; }

        public ImageThumbnailListDto(Image image)
        {
            ContainerName = image?.ContainerName;
            FileName = image?.ThumbnailName;
        }
    }
}