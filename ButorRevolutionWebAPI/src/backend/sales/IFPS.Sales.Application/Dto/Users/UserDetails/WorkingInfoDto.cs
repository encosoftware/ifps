using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WorkingInfoDto
    {
        public List<WorkingHourDto> WorkingHours { get; set; }
        public List<VenueDto> Offices { get; set; }
        public UserDto Supervisor { get; set; }
        public List<UserDto> Supervisees { get; set; }
        public List<string> Teams { get; set; }
        public int MinDiscountPercent { get; set; }
        public int MaxDiscountPercent { get; set; }
    }
}
