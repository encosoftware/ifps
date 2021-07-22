using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IMaterialAppService
    {
        Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdownByCategoryAsync(int categoryId);
        Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdownAsync();
    }
}
