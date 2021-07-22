using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IClaimAppService
    {
        Task<List<string>> GetClaimsListAsync();
        Task<List<ClaimGroupDto>> GetClaimsWithGroupsListAsync();
    }
}
