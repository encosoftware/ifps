using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class UpdateWorkingInfoDto
    {
        public List<WorkingHourDto> WorkingHours { get; set; }
        public List<int> OfficeIds { get; set; }
        public int? SupervisorUserId { get; set; }
        public int MinDiscountPercent { get; set; }
        public int MaxDiscountPercent { get; set; }
    }
}
