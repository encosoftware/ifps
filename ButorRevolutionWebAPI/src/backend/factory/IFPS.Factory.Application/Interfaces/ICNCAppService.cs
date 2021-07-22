using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICncAppService
    {
        Task<PagedListDto<CNCListDto>> CncListAsync(CncFilterDto dto);
    }
}
