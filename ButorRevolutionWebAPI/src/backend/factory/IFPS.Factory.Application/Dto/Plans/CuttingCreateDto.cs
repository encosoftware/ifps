using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingCreateDto
    {
        public int TopLeftX { get; set; }
        public int TopLeftY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public CuttingCreateDto()
        {

        }

        public Cutting CreateModelObject(int layoutPlanId)
        {
            return new Cutting(layoutPlanId)
            {
                TopLeftX = TopLeftX,
                TopLeftY = TopLeftY,
                Width = Width,
                Height = Height
            };
        }
    }
}
