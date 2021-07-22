using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitUpdateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }

        public FurnitureUnit UpdateModelObject(FurnitureUnit furnitureUnit)
        {
            furnitureUnit.Description = Description;
            furnitureUnit.CategoryId = CategoryId;
            furnitureUnit.Width = Width;
            furnitureUnit.Height = Height;
            furnitureUnit.Depth = Depth;
            return furnitureUnit;
        }
    }
}
