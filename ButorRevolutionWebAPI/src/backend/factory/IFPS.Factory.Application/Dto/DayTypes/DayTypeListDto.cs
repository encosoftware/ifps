using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
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