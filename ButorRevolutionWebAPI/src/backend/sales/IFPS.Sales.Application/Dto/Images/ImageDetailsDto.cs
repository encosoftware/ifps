using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ImageDetailsDto
    {
        public string ContainerName { get; set; }
        public string FileName { get; set; }

        public ImageDetailsDto(Image image)
        {
            ContainerName = image?.ContainerName;
            FileName = image?.FileName;
        }
    }
}
