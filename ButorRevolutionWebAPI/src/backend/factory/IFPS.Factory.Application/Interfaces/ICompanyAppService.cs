using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICompanyAppService
    {
        Task<PagedListDto<CompanyListDto>> GetCompaniesAsync(CompanyFilterDto filter);
        Task<CompanyDetailsDto> GetCompanyDetailsAsync(int id);
        Task<int> CreateCompanyAsync(CompanyCreateDto userDto);
        Task UpdateCompanyAsync(int id, CompanyUpdateDto userDto);
        Task DeleteCompanyAsync(int id);
        Task<List<SupplierDropdownListDto>> GetSuppliersForDropdownAsync();
    }
}
