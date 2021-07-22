using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Sales.Domain.Model
{
    public class Dimension : ValueObject<Dimension>
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }

        public Dimension(double width, double length, double thickness)
        {
            Width = width;
            Length = length;
            Thickness = thickness;
        }
    }
}
