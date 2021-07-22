using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public int OptimalCrew { get; set; }
        public int? MachineId { get; set; }
        public int? WorkStationTypeId { get; set; }
        public bool? Status { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(WorkStation.Name) }
            };
        }
    }
}
