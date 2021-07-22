using System.Collections.Generic;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces.Material;
using IFPS.Sales.Domain.Repositories.Material;

namespace IFPS.Sales.Application.Services.Material
{
    public class BoardMaterialAppService : ApplicationService, IBoardMaterialAppService
    {
        private readonly IBoardMaterialRepository boardMaterialRepository;

        public BoardMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IBoardMaterialRepository boardMaterialRepository
            )
            : base(aggregate)
        {
            this.boardMaterialRepository = boardMaterialRepository;
        }

        public async Task<List<BoardMaterialsForDropdownDto>> GetBoardMaterialsAsync()
        {
            var boards = await boardMaterialRepository.GetAllListAsync();

            var boardList = new List<BoardMaterialsForDropdownDto>();

            foreach(var board in boards)
            {
                var newBoard = new BoardMaterialsForDropdownDto(board.Id, board.Description);

                boardList.Add(newBoard);
            }

            return boardList;
        }
    }
}
