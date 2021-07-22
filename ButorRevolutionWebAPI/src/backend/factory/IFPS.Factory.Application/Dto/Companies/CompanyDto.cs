using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static CompanyDto FromModel(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
            };
        }
    }
}