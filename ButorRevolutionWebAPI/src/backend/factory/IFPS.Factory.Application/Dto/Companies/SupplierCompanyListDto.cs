using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class SupplierCompanyListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SupplierCompanyListDto(Company company)
        {
            Id = company.Id;
            Name = company.Name;
        }
    }
}
