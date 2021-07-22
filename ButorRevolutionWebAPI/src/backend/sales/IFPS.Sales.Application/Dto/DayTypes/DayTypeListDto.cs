using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class DayTypeListDto
    {
        public int Id { get; set; }
        public DayTypeEnum DayType { get; set; }
        public string Translation { get; set; }
        public int Order { get; set; }

        public DayTypeListDto(DayType dayType)
        {
            Id = dayType.Id;
            DayType = dayType.Type;
            Order = dayType.Order;
            Translation = dayType.CurrentTranslation?.Name;
        }
    }

}
