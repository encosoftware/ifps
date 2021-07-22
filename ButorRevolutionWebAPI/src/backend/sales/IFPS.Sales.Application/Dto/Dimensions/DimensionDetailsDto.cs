using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class DimensionDetailsDto
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }

        public DimensionDetailsDto(Dimension dimension)
        {
            Width = dimension.Width;
            Length = dimension.Length;
            Thickness = dimension.Thickness;
        }
    }
}
