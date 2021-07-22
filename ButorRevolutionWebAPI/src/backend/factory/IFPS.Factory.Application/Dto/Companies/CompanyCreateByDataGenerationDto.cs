using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyCreateByDataGenerationDto
    {
        public string BrandName { get; set; }

        public Company CreateModelObject()
        {
            return new Company(BrandName,4);
        }
    }
}
