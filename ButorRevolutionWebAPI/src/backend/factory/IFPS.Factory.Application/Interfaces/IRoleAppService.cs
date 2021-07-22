using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IRoleAppService
    {
        Task<int> CreateRoleAsync(RoleCreateDto roleCreateDto);
        Task<List<RoleListDto>> GetRolesAsync();
        Task<RoleDetailsDto> GetRoleDetailsAsync(int id);        
        Task UpdateRoleAsync(int id, RoleUpdateDto userDto);
        Task<List<RoleClaimsDto>> GetRolesWithClaimsAsync();
    }
}
