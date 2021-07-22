using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialCreateDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }
        public bool IsOptional { get; set; }
        public bool IsRequiredForAssembly { get; set; }
        public PriceCreateDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public AccessoryMaterial CreateModelObject()
        {
            return new AccessoryMaterial(IsOptional, IsRequiredForAssembly, Code, TransactionMultiplier)
            {
                Description = Description,
                CategoryId = CategoryId,
            };
        }
    }
}
