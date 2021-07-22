using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ImageListDto
    {
        public string ContainerName { get; set; }
        public string FileName { get; set; }

        public ImageListDto(Image image)
        {
            ContainerName = image.ContainerName;
            FileName = image.FileName;
        }
    }
}
