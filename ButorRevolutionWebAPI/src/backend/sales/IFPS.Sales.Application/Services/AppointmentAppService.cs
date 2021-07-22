using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Dto.Appointments;
using IFPS.Sales.Application.Exceptions;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class AppointmentAppService : ApplicationService, IAppointmentAppService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IUserTeamRepository userTeamRepository;
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;

        public AppointmentAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IAppointmentRepository appointmentRepository,
            IUserTeamRepository userTeamRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository)
            : base(aggregate)
        {
            this.appointmentRepository = appointmentRepository;
            this.userTeamRepository = userTeamRepository;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<int> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto)
        {
            var newAppointment = appointmentCreateDto.CreateModelObject();
            if (appointmentCreateDto.MeetingRoomId == null)
            {
                newAppointment.Address = appointmentCreateDto.AddressCreateDto.CreateModelObject();
            }
            else
            {
                newAppointment.Address = new Address(0, "", "", 1);
                newAppointment.MeetingRoomId = appointmentCreateDto.MeetingRoomId;
            }

            var partnerUser = await userRepository.SingleAsync(ent => ent.Id == appointmentCreateDto.PartnerId);
            if (partnerUser.IsTechnicalAccount)
            {
                await SetOrderStatusByPartnerAsync(appointmentCreateDto.OrderId.Value, appointmentCreateDto.PartnerId);
            }

            await appointmentRepository.InsertAsync(newAppointment);
            await unitOfWork.SaveChangesAsync();
            return newAppointment.Id;
        }

        public async Task<int> CreateAppointmentForOderAsync(Guid orderId, AppointmentCreateDto appointmentCreateDto)
        {
            var newAppointment = appointmentCreateDto.CreateModelObject();
            newAppointment.OrderId = orderId;
            if (appointmentCreateDto.MeetingRoomId != null)
            {
                newAppointment.Address = new Address(0, "", "", 1);
                newAppointment.MeetingRoomId = appointmentCreateDto.MeetingRoomId;
            }
            else
            {
                newAppointment.Address = appointmentCreateDto.AddressCreateDto.CreateModelObject();
            }

            var partnerUser = await userRepository.SingleAsync(ent => ent.Id == appointmentCreateDto.PartnerId);
            if (partnerUser.IsTechnicalAccount)
            {
                await SetOrderStatusByPartnerAsync(orderId, appointmentCreateDto.PartnerId);
            }

            await appointmentRepository.InsertAsync(newAppointment);
            await unitOfWork.SaveChangesAsync();
            return newAppointment.Id;
        }

        private async Task SetOrderStatusByPartnerAsync(Guid orderId, int partnerId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.CurrentTicket.OrderState, ent => ent.Customer.User.CurrentVersion);

            switch (order.CurrentTicket.OrderState.State)
            {
                case OrderStateEnum.WaitingForShippingAppointmentReservation:
                    order.SetWaitingForShippingState(partnerId);
                    break;
                case OrderStateEnum.Delivered:
                    order.SetWaitingForInstallationState(partnerId);
                    break;
                case OrderStateEnum.WaitingForOnSiteSurveyAppointmentReservation:
                    order.SetWaitingForOnSiteSurveyState(partnerId);
                    break;
                default:
                    throw new IFPSAppException("Cannot create the appointment with status of the order!");
            };
        }

        public async Task DeleteAppointmentAsync(int id, int partnerId)
        {
            await SetOrderStatusAfterAppointmentDeleted(id, partnerId);
            await appointmentRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task SetOrderStatusAfterAppointmentDeleted(int appointmentId, int partnerId)
        {
            var appointment = await appointmentRepository.SingleIncludingAsync(ent => ent.Id == appointmentId, ent => ent.Order.CurrentTicket.OrderState, ent => ent.Order.SalesPerson,
                                                    ent => ent.Order.Customer.User.CurrentVersion);
            if (appointment.Order != null)
            {
                switch (appointment.Order.CurrentTicket.OrderState.State)
                {
                    case OrderStateEnum.WaitingForShipping:
                        appointment.Order.SetWaitingForShippingAppointmentReservationState(partnerId);
                        break;
                    case OrderStateEnum.WaitingForInstallation:
                        appointment.Order.SetDeliverdState(partnerId);
                        break;
                    case OrderStateEnum.WaitingForOnSiteSurvey:
                        appointment.Order.SetWaitingForOnSiteSurveyAppointmentReservationState();
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task<AppointmentDetailsDto> GetAppointmentDetailsAsync(int id)
        {
            var appointment = await appointmentRepository
                .SingleIncludingAsync(ent => ent.Id == id, ent => ent.Customer.CurrentVersion, ent => ent.MeetingRoom.Venue);
            return new AppointmentDetailsDto(appointment);
        }

        public async Task<AppointmentDetailsDto> GetCustomerAppointmentDetailsAsync(int id, int customerId)
        {
            var appointment = await appointmentRepository
                .SingleIncludingAsync(ent => ent.Id == id && ent.CustomerId == customerId,
                    ent => ent.Customer.CurrentVersion, ent => ent.MeetingRoom.Venue);
            return new AppointmentDetailsDto(appointment);
        }

        public async Task<AppointmentDetailsDto> GetPartnerAppointmentDetailsAsync(int id, int partnerId)
        {
            var appointment = await appointmentRepository
                .SingleIncludingAsync(ent => ent.Id == id && ent.PartnerId == partnerId,
                    ent => ent.Customer.CurrentVersion, ent => ent.MeetingRoom.Venue);
            return new AppointmentDetailsDto(appointment);
        }

        public async Task<List<AppointmentListDto>> GetAppointmentsByOrderAsync(Guid orderId)
        {
            var appointments = await appointmentRepository
                .GetAllListAsync<AppointmentListDbo>(ent => ent.OrderId == orderId, AppointmentListDbo.Projection);

            return appointments.Select(ent => new AppointmentListDto(ent)).ToList();
        }

        public async Task<List<AppointmentListDto>> GetAppointmentsByDateRangeAsync(int userId, AppointmentDateRangeDto appointmentDateRangeDto)
        {
            List<int> userIds = await userTeamRepository.GetTechnicalUserIdsByUserIdAsync(userId);
            userIds.Add(userId);

            var user = await userRepository.GetUserDivisionAsync(userId);
            Expression<Func<Appointment, bool>> predicate = null;

            if (user.Roles.Any(ent => ent.Role.Division.DivisionType == DivisionTypeEnum.Admin))
            {
                predicate = ent => ent.ScheduledDateTime.From.Date >= appointmentDateRangeDto.From.Date
                                                                && ent.ScheduledDateTime.To.Date <= appointmentDateRangeDto.To.Date;
            }
            else
            {
                predicate = ent => ent.ScheduledDateTime.From.Date >= appointmentDateRangeDto.From.Date
                                                                && ent.ScheduledDateTime.To.Date <= appointmentDateRangeDto.To.Date
                                                                && (userIds.Contains(ent.CustomerId.Value) || userIds.Contains(ent.PartnerId) ||
                                                                    userIds.Contains(ent.Order.SalesPerson.UserId));
            }

            var appointments = await appointmentRepository.GetAllListAsync(predicate, AppointmentListDbo.Projection);
            return appointments.Select(ent => new AppointmentListDto(ent)).ToList();
        }

        public async Task<List<AppointmentListDto>> GetAppointmentsByDateRangeAndMeetingRoomAsync(int meetingRoomId, AppointmentDateRangeDto appointmentDateRangeDto)
        {
            Expression<Func<Appointment, bool>> predicate = ent => ent.ScheduledDateTime.From.Date >= appointmentDateRangeDto.From.Date
                                                                && ent.ScheduledDateTime.To.Date <= appointmentDateRangeDto.To.Date
                                                                && ent.MeetingRoomId == meetingRoomId;

            var appointments = await appointmentRepository.GetAllListAsync<AppointmentListDbo>(predicate, AppointmentListDbo.Projection);
            return appointments.Select(ent => new AppointmentListDto(ent)).ToList();
        }

        public async Task UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto)
        {
            var appointment = await appointmentRepository.SingleAsync(ent => ent.Id == id);
            appointment = appointmentUpdateDto.UpdateModelObject(appointment);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAppointmentDateAsync(int id, AppointmentDateRangeDto appointmentDateRangeDto)
        {
            var appointment = await appointmentRepository.SingleAsync(ent => ent.Id == id);
            appointment.ScheduledDateTime = appointmentDateRangeDto.CreateModelObject();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
