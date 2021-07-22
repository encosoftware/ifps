using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IProductionProcessAppService
    {
        Task SetProcessStatusAsync(int id, int userId);
    }
}
