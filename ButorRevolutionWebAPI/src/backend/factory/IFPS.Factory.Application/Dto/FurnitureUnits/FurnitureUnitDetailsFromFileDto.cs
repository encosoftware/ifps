using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitDetailsFromFileDto
    {
        public PriceCreateDto Price { get; set; }

        public PriceCreateDto MaterialCost { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public ImageCreateDto Image { get; set; }

        public FurnitureUnitDetailsFromFileDto()
        {

        }

        public void SetSize(FurnitureUnit furnitureUnit)
        {
            furnitureUnit.Width = Width;
            furnitureUnit.Height = Height;
            furnitureUnit.Depth = Depth;
        }
    }
}
