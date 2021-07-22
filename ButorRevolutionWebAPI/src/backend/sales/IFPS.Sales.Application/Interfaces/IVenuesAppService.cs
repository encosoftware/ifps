using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IVenuesAppService
    {
        Task<List<VenueDto>> SearchVenueAsync(string name, int? companyId, int take);
        Task<int> CreateVenueAsync(VenueCreateDto dto);
        Task<PagedListDto<VenueListDto>> GetVenuesListAsync(VenueFilterDto dto);

        Task<List<VenueDto>> GetVenueNamesAsync();
        Task<List<MeetingRoomNameListDto>> GetMeetingRoomNamesAsync(int venueId);

        Task<List<MeetingRoomAppointmentsDto>> GetMeetingRoomsByCompanyAsync(int companyId);
        Task<VenueDetailsDto> GetVenueDetailsAsync(int id);
        Task UpdateVenueAsync(int id, VenueUpdateDto dto);
        Task DeleteVenueAsync(int id);
        Task ActivateOrDeactivateVenueAsync(int id);
    }
}
