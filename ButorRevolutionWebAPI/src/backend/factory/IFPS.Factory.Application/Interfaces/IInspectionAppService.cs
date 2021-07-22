using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IInspectionAppService
    {
        Task<int> CreateInspectionAsync(InspectionCreateDto inspectionCreateDto);
        Task<PagedListDto<InspectionListDto>> GetInspectionsAsync(InspectionFilterDto inspectionFilterDto);
        Task<InspectionDetailsDto> GetInspectionAsync(int id);
        Task UpdateInspectionAsync(int id, InspectionUpdateDto inspectionUpdateDto);
        Task<ReportDetailsDto> GetInspectionReportAsync(int id);
        Task UpdateInspectionReportAsync(int id, ReportUpdateDto reportUpdateDto);
        Task DeleteInspectionAsync(int id);
        Task CloseInspectionReportAsync(int id);
    }
}
