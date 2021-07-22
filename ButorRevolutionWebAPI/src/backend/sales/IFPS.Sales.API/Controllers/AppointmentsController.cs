using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Dto.Appointments;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : IFPSControllerBase
    {
        private const string OPNAME = "Appointments";

        private readonly IAppointmentAppService appointmentService;

        public AppointmentsController(IAppointmentAppService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        //GET appointment by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<AppointmentDetailsDto> GetAppointmentById(int id)
        {
            return appointmentService.GetAppointmentDetailsAsync(id);
        }

        //Get appointment by id for customer only
        [HttpGet("own/{id}")]
        [Authorize(Policy = "GetCustomerAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<AppointmentDetailsDto> GetCustomerAppointmentById(int id)
        {
            return appointmentService.GetCustomerAppointmentDetailsAsync(id, GetCallerId());
        }

        // GET appointment by id for partner only
        [HttpGet("partner/{appointmentId}")]
        [Authorize(Policy = "GetPartnerAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<AppointmentDetailsDto> GetPartnerAppointmentById(int appointmentId)
        {
            return appointmentService.GetPartnerAppointmentDetailsAsync(appointmentId, GetCallerId());
        }

        //GET appointments by orderId
        [HttpGet("orders/{orderId}")]
        //TODO: több külön endpoint és CallerId szűrés, hogy szebb legyen
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AppointmentListDto>> GetAppointmentByOrder(Guid orderId)
        {
            return appointmentService.GetAppointmentsByOrderAsync(orderId);
        }

        //GET appointments by date
        [HttpGet("range/{userId}")]
        [Authorize(Policy = "GetAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AppointmentListDto>> GetAppointmentsByDateRange(int userId, [FromQuery]AppointmentDateRangeDto appointmentDateRangeDto)
        {
            return appointmentService.GetAppointmentsByDateRangeAsync(userId, appointmentDateRangeDto);
        }

        //GET appointments by date for customer only
        [HttpGet("range/customer")]
        [Authorize(Policy = "GetCustomerAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AppointmentListDto>> GetCustomerAppointmentsByDateRange([FromQuery]AppointmentDateRangeDto appointmentDateRangeDto)
        {
            return appointmentService.GetAppointmentsByDateRangeAsync(GetCallerId(), appointmentDateRangeDto);
        }

        //GET appointments by date for partner only
        [HttpGet("range/partner")]
        [Authorize(Policy = "GetPartnerAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AppointmentListDto>> GetPartnerAppointmentsByDateRange([FromQuery]AppointmentDateRangeDto appointmentDateRangeDto)
        {
            return appointmentService.GetAppointmentsByDateRangeAsync(GetCallerId(), appointmentDateRangeDto);
        }

        //GET appointments by date and meetingroom
        [HttpGet("range/meetingrooms/{meetingRoomId}")]
        [Authorize(Policy = "GetAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AppointmentListDto>> GetAppointmentsByDateRangeAndMeetingRoom(int meetingRoomId, [FromQuery]AppointmentDateRangeDto appointmentDateRangeDto)
        {
            return appointmentService.GetAppointmentsByDateRangeAndMeetingRoomAsync(meetingRoomId, appointmentDateRangeDto);
        }

        // POST: api/appointments
        [HttpPost]
        [Authorize(Policy = "UpdateAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateAppointment([FromBody] AppointmentCreateDto appointmentCreateDto)
        {
            return appointmentService.CreateAppointmentAsync(appointmentCreateDto);
        }

        // POST appointment for order
        [HttpPost("{orderId}")]
        [Authorize(Policy = "UpdateAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateAppointmentForOrder(Guid orderId, [FromBody] AppointmentCreateDto createDto)
        {
            return appointmentService.CreateAppointmentForOderAsync(orderId, createDto);
        }

        // PUT: api/appointments/
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateAppointment(int id, [FromBody]AppointmentUpdateDto updateDto)
        {
            return appointmentService.UpdateAppointmentAsync(id, updateDto);
        }

        // PUT: update appointment date
        [HttpPut("{id}/dates")]
        [Authorize(Policy = "UpdateAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateAppointmentDate(int id, [FromBody]AppointmentDateRangeDto appointmentDateRangeDto)
        {
            return appointmentService.UpdateAppointmentDateAsync(id, appointmentDateRangeDto);
        }

        // DELETE: api/appointments/
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteAppointments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteAppointment(int id)
        {
            return appointmentService.DeleteAppointmentAsync(id, GetCallerId());
        }
    }
}