using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface ICompanyAppService
    {
        Task<PagedListDto<CompanyListDto>> GetCompaniesAsync(CompanyFilterDto filter);
        Task<CompanyDetailsDto> GetCompanyDetailsAsync(int id);
        Task<int> CreateCompanyAsync(CompanyCreateDto userDto);
        Task UpdateCompanyAsync(int id, CompanyUpdateDto userDto);
        Task DeleteCompanyAsync(int id);
        Task<string> GetCompanyNameByIdAsync(int id);
    }
}
