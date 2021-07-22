using System.Collections.Generic;
using System.Threading.Tasks;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetAllAppointmentsByPartnerAsync(List<int> userIds);
    }
}
