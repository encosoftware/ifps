using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IRoleAppService
    {
        Task<int> CreateRoleAsync(RoleCreateDto roleCreateDto);
        Task<List<RoleListDto>> GetRolesAsync();
        Task<RoleDetailsDto> GetRoleDetailsAsync(int id);        
        Task UpdateRoleAsync(int id, RoleUpdateDto userDto);
        Task<List<RoleClaimsDto>> GetRolesWithClaimsAsync();

        Task<List<DivisionTypeEnum>> GetDivisionListByRoleIds(List<int> roleIds);
    }
}
