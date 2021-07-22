using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IDecorBoardMaterialAppService
    {
        Task CreateBoardsFromFileAsync(string container, string fileName);
    }
}
