using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class ProductionStatusDetailsDto
    {
        public double CuttingsStatus { get; set; }
        public double CncStatus { get; set; }
        public double EdgebandingStatus { get; set; }

        public ProductionStatusDetailsDto(List<ProductionProcess> productionProcesses)
        {
            if (productionProcesses.Count > 0)
            {
                var cuttings = productionProcesses.Where(ent => ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Layout).ToList();
                if (cuttings.Count > 0)
                {
                    CuttingsStatus = SetStatus(cuttings);
                }

                var cncs = productionProcesses.Where(ent => ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Cnc).ToList();
                if (cncs.Count > 0)
                {
                    CncStatus = SetStatus(cncs);
                }

                var edgebandings = productionProcesses.Where(ent => ent.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Edging).ToList();
                if (edgebandings.Count > 0)
                {
                    EdgebandingStatus = SetStatus(edgebandings);
                }
            }
        }

        private double SetStatus(List<ProductionProcess> productionProcesses)
        {
            var processCount = productionProcesses.Count;
            double completed = 0;
            foreach (var process in productionProcesses)
            {
                if(process.EndTime.HasValue && process.EndTime != DateTime.MinValue)
                {
                    completed += 1;
                }
            }
            return Math.Round((completed / processCount) * 100, 2);
        }
    }
}
