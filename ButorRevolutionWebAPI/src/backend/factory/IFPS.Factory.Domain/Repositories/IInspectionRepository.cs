using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IInspectionRepository : IRepository<Inspection>
    {
        Task<Inspection> GetInspectionReportWithIncludesAsync(int id);
    }
}