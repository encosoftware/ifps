using IFPS.Sales.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces.Material
{
    public interface IBoardMaterialAppService
    {
        Task<List<BoardMaterialsForDropdownDto>> GetBoardMaterialsAsync();
    }
}
