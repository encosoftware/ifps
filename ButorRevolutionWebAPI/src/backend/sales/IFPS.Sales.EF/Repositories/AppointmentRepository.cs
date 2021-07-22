using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class AppointmentRepository : EFCoreRepositoryBase<IFPSSalesContext, Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IFPSSalesContext context) : base(context)
        { }
        protected override List<Expression<Func<Appointment, object>>> DefaultIncludes => new List<Expression<Func<Appointment, object>>>
        {

        };

        public async Task<List<Appointment>> GetAllAppointmentsByPartnerAsync(List<int> userIds)
        {
            var result = await GetAll()
                .Where(ent => userIds.Contains(ent.PartnerId) && ent.OrderId.HasValue).ToListAsync();

            return result ?? throw new EntityNotFoundException(typeof(Appointment), userIds);
        }     
    }
}
