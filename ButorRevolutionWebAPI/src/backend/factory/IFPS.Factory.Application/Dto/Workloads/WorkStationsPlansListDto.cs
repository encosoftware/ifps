using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationsPlansListDto
    {
        public List<WorkStationsPlansDetailsDto> Cuttings { get; set; }
        public List<WorkStationsPlansDetailsDto> Cncs { get; set; }
        public List<WorkStationsPlansDetailsDto> Edgebandings { get; set; }
        public List<WorkStationsPlansDetailsDto> Assemblies { get; set; }
        public List<WorkStationsPlansDetailsDto> Sortings { get; set; }
        public List<WorkStationsPlansDetailsDto> Packings { get; set; }

        public WorkStationsPlansListDto()
        {
            Cuttings = new List<WorkStationsPlansDetailsDto>();
            Cncs = new List<WorkStationsPlansDetailsDto>();
            Edgebandings = new List<WorkStationsPlansDetailsDto>();
            Assemblies = new List<WorkStationsPlansDetailsDto>();
            Sortings = new List<WorkStationsPlansDetailsDto>();
            Packings = new List<WorkStationsPlansDetailsDto>();
        }

        public void SetProperty(WorkStation workStation)
        {
            switch(workStation.WorkStationType.StationType)
            {
                case WorkStationTypeEnum.Layout:
                    Cuttings.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
                case WorkStationTypeEnum.Cnc:
                    Cncs.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
                case WorkStationTypeEnum.Edging:
                    Edgebandings.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
                case WorkStationTypeEnum.Assembly:
                    Assemblies.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
                case WorkStationTypeEnum.Sorting:
                    Sortings.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
                case WorkStationTypeEnum.Packing:
                    Packings.Add(new WorkStationsPlansDetailsDto(workStation));
                    break;
            }
        }
    }
}
