using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitCreateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }

        public FurnitureUnit CreateModelObject()
        {
            return new FurnitureUnit(Code, Width, Height, Depth) { Description = Description, CategoryId = CategoryId };
        }
    }
}