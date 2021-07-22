using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Dto.Appointments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IAppointmentAppService
    {
        Task<int> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto);
        Task<int> CreateAppointmentForOderAsync(Guid orderId, AppointmentCreateDto appointmentCreateDto);
        Task UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto);
        Task<List<AppointmentListDto>> GetAppointmentsByOrderAsync(Guid orderId);
        Task<AppointmentDetailsDto> GetAppointmentDetailsAsync(int id);
        Task<AppointmentDetailsDto> GetCustomerAppointmentDetailsAsync(int id, int customerId);
        Task<AppointmentDetailsDto> GetPartnerAppointmentDetailsAsync(int appointmentId, int partnerId);
        Task<List<AppointmentListDto>> GetAppointmentsByDateRangeAsync(int userId, AppointmentDateRangeDto appointmentDateRangeDto);
        Task<List<AppointmentListDto>> GetAppointmentsByDateRangeAndMeetingRoomAsync(int meetingRoomId, AppointmentDateRangeDto appointmentDateRangeDto);
        Task UpdateAppointmentDateAsync(int id, AppointmentDateRangeDto appointmentDateRangeDto);
        Task DeleteAppointmentAsync(int id, int partnerId);
    }
}
