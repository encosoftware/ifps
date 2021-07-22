using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationsForCuttingListDto
    {
        public List<LayoutWorkStationsListDto> LayoutWorkstations { get; set; }
        public List<CncWorkStationsListDto> CncWorkstations { get; set; }
        public List<EdgingWorkStationsListDto> EdgingWorkstations { get; set; }
        public List<SortingWorkStationsListDto> SortingWorkstations { get; set; }
        public List<AssemblyWorkStationsListDto> AssemblyWorkstations { get; set; }
        public List<PackingWorkStationsListDto> PackingWorkstations { get; set; }

        public WorkStationsForCuttingListDto()
        {
            LayoutWorkstations = new List<LayoutWorkStationsListDto>();
            CncWorkstations = new List<CncWorkStationsListDto>();
            EdgingWorkstations = new List<EdgingWorkStationsListDto>();
            SortingWorkstations = new List<SortingWorkStationsListDto>();
            AssemblyWorkstations = new List<AssemblyWorkStationsListDto>();
            PackingWorkstations = new List<PackingWorkStationsListDto>();
        }

        public void SetWorkStationProperties(WorkStation workStation)
        {
            switch(workStation.WorkStationType.StationType)
            {
                case WorkStationTypeEnum.Layout:
                    LayoutWorkstations.Add(new LayoutWorkStationsListDto(workStation));
                    break;
                case WorkStationTypeEnum.Cnc:
                    CncWorkstations.Add(new CncWorkStationsListDto(workStation));
                    break;
                case WorkStationTypeEnum.Edging:
                    EdgingWorkstations.Add(new EdgingWorkStationsListDto(workStation));
                    break;
                case WorkStationTypeEnum.Sorting:
                    SortingWorkstations.Add(new SortingWorkStationsListDto(workStation));
                    break;
                case WorkStationTypeEnum.Assembly:
                    AssemblyWorkstations.Add(new AssemblyWorkStationsListDto(workStation));
                    break;
                case WorkStationTypeEnum.Packing:
                    PackingWorkstations.Add(new PackingWorkStationsListDto(workStation));
                    break;
                default:
                    break;
            }
        }
    }
}
