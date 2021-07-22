using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class DimensionUpdateDto
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }

        public DimensionUpdateDto()
        {

        }

        public Dimension CreateModelObject()
        {
            return new Dimension(Width, Length, Thickness);
        }
    }
}
