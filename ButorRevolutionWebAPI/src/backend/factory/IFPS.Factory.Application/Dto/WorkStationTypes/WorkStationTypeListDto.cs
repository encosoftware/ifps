using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationTypeListDto
    {
        public int Id { get; set; }

        public WorkStationTypeEnum Type { get; set; }

        public string Translation { get; set; }

        public WorkStationTypeListDto(WorkStationType type)
        {
            Id = type.Id;
            Type = type.StationType;
            Translation = type.CurrentTranslation?.Name;
        }
    }
}
