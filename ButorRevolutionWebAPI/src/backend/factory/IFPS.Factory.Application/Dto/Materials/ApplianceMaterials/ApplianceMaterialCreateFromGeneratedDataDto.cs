using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ApplianceMaterialCreateFromGeneratedDataDto
    {
        public string Description { get; set; }

        public CompanyCreateByDataGenerationDto Brand { get; set; }

        public string Code { get; set; }

        public PriceCreateDto Price { get; set; }

        public ApplianceMaterialCreateFromGeneratedDataDto()
        {

        }

        public ApplianceMaterial CreateModelObject()
        {
            return new ApplianceMaterial(Code)
            {
                Brand = Brand.CreateModelObject(),
                Description = Description,
                SellPrice = Price.CreateModelObject()
            };
        }
    }
}
