using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IWorktopBoardMaterialAppService
    {
        Task<WorktopBoardMaterialDetailsDto> GetWorktopBoardMaterialAsync(Guid id);
        Task<PagedListDto<WorktopBoardMaterialListDto>> GetWorktopBoardMaterialsAsync(WorktopBoardMaterialFilterDto filterDto);
        Task<Guid> CreateWorktopBoardMaterialAsync(WorktopBoardMaterialCreateDto worktopBoardMaterialCreateDto);
        Task UpdateWorktopBoardMaterialAsync(Guid id, WorktopBoardMaterialUpdateDto worktopBoardMaterialDto);
        Task DeleteMaterialAsync(Guid id);
    }
}
