using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class FoilMaterialCreateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }
        public double Thickness { get; set; }
        public PriceCreateDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public FoilMaterial CreateModelObject()
        {
            return new FoilMaterial(Code, TransactionMultiplier)
            {
                Description = Description,
                Thickness = Thickness,
            };
        }
    }
}
