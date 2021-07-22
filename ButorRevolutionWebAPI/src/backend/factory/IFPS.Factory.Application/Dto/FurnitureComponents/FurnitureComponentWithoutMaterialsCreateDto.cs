using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureComponentWithoutMaterialsCreateDto
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int Amount { get; set; }

        public FurnitureComponent CreateModelObject()
        {
            return new FurnitureComponent(Name, Width, Length, Amount) { };
        }
    }
}
