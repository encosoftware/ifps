using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IDecorBoardMaterialAppService
    {
        Task<DecorBoardMaterialDetailsDto> GetDecorBoardMaterialAsync(Guid id);
        Task<PagedListDto<DecorBoardMaterialListDto>> GetDecorBoardMaterialsAsync(DecorBoardMaterialFilterDto filterDto);
        Task<Guid> CreateDecorBoardMaterialAsync(DecorBoardMaterialCreateDto decorBoardMaterialCreateDto);
        Task UpdateDecorBoardMaterialAsync(Guid id, DecorBoardMaterialUpdateDto decorBoardMaterialDto);
        Task DeleteMaterialAsync(Guid id);
        Task<List<DecorBoardMaterialCodesDto>> GetDecorBoardMaterialCodesAsync();
        Task<List<DecorBoardMaterialWithImageDto>> GetDecorBoardMaterialForDropdownAsync();
    }
}
