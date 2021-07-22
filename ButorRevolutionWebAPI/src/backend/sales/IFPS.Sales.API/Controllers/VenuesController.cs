using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/venues")]
    [ApiController]
    public class VenuesController : IFPSControllerBase
    {
        private const string OPNAME = "Venues";

        private readonly IVenuesAppService venuesAppService;

        public VenuesController(IVenuesAppService service)
        {
            this.venuesAppService = service;
        }

        // GET office search
        [HttpGet("search")]
        [Authorize(Policy = "GetVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<VenueDto>> Search([FromQuery] string name, [FromQuery] int? companyId, [FromQuery] int take = 10)
        {
            return venuesAppService.SearchVenueAsync(name, companyId, take);
        }

        // POST : create a new venue
        [HttpPost]
        [Authorize(Policy = "UpdateVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateVenue([FromBody]VenueCreateDto dto)
        {
            return venuesAppService.CreateVenueAsync(dto);
        }

        // GET : get all venues
        [HttpGet]
        [Authorize(Policy = "GetVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<VenueListDto>> List([FromQuery]VenueFilterDto filter)
        {
            return venuesAppService.GetVenuesListAsync(filter);
        }
        
        // GET : get meetingrooms by company
        [HttpGet("companies/{companyId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MeetingRoomAppointmentsDto>> GetMeetingRoomsByCompany(int companyId)
        {
            return venuesAppService.GetMeetingRoomsByCompanyAsync(companyId);
        }

        // GET : get venue by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<VenueDetailsDto> Details(int id)
        {
            return venuesAppService.GetVenueDetailsAsync(id);
        }

        // GET : get venue name list
        [HttpGet("names")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<VenueDto>> GetVenueNames()
        {
            return venuesAppService.GetVenueNamesAsync();
        }

        // GET : get meetingroom name list
        [HttpGet("{id}/meetingrooms")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<MeetingRoomNameListDto>> GetMeetingRoomNames(int id)
        {
            return venuesAppService.GetMeetingRoomNamesAsync(id);
        }

        // PUT : update an existing venue by id
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateVenue(int id, [FromBody] VenueUpdateDto dto)
        {
            return venuesAppService.UpdateVenueAsync(id, dto);
        }

        // DELETE : delete venue by id
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteVenue(int id)
        {
            return venuesAppService.DeleteVenueAsync(id);
        }

        // activate or deactivate venue
        [HttpPost("deactivate/{id}")]
        [Authorize(Policy = "UpdateVenues")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ActivateOrDeactivateVenue(int id)
        {
            return venuesAppService.ActivateOrDeactivateVenueAsync(id);
        }
    }
}