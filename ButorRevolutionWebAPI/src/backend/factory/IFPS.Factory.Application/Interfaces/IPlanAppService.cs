using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IPlanAppService
    {
        Task SetPlanProductionProcessTimeAsync(string ipAddress, int cfcId);
    }
}
