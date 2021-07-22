using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IMessageChannelParticipantRepository messageChannelParticipantRepository;
        
        public OrderService(IAppointmentRepository appointmentRepository,
            IOrderRepository orderRepository,
            IMessageChannelParticipantRepository messageChannelParticipantRepository)
        {
            this.orderRepository = orderRepository;
            this.appointmentRepository = appointmentRepository;
            this.messageChannelParticipantRepository = messageChannelParticipantRepository;
        }

        public async Task<List<int>> GetOrderAvailableContactIds(Guid orderId)
        {
            var contactList = new List<int>();

            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId,
                ent => ent.SalesPerson, ent => ent.Customer);

            contactList.Add(order.SalesPerson.UserId);
            contactList.Add(order.Customer.UserId);

            Expression<Func<Appointment, int?>> partners = x => x.PartnerId;

            var orderAppointmentContacts = await appointmentRepository.GetAllListAsync(ent => ent.OrderId == orderId, partners);
            contactList.AddRange(orderAppointmentContacts.Where(ent => ent.HasValue).Select(ent => ent.Value));
            
            return contactList.Distinct().ToList();
        }
    }
}
