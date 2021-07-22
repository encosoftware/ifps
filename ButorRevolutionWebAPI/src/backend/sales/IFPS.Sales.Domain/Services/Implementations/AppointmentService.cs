using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    class AppointmentService : IAppointmentService
    {
        private readonly IUserTeamRepository userTeamRepository;
        private readonly IIdentityService identityService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentService(IUserTeamRepository userTeamRepository,
            IIdentityService identityService,
            IAppointmentRepository appointmentRepository)
        {
            this.userTeamRepository = userTeamRepository;
            this.identityService = identityService;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<Guid>> GetOrderIdsByPartnerIdAsync()
        {
            int userId = identityService.GetCurrentUserId();
            List<int> userIds = await userTeamRepository.GetTechnicalUserIdsByUserIdAsync(userId);
            userIds.Add(userId);

            var appointments = await appointmentRepository.GetAllAppointmentsByPartnerAsync(userIds);
            return appointments.Select(ent => ent.OrderId.Value).Distinct().ToList();
        }
    }
}
