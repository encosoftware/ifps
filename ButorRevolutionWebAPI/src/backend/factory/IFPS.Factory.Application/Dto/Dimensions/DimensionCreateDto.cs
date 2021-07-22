using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class DimensionCreateDto
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }

        public DimensionCreateDto()
        {
        }

        public Dimension CreateModelObject()
        {
            return new Dimension(Width, Length, Thickness);
        }
    }
}