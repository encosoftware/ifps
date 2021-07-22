using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionDetailsDto
    {
        public DateTime InspectedOn { get; set; }
        public string ReportName { get; set; }
        public int ReportId { get; set; }
        public int StorageId { get; set; }
        public List<int> DelegationIds { get; set; }

        public InspectionDetailsDto(Inspection inspection)
        {
            InspectedOn = inspection.InspectedOn;
            ReportName = inspection.Report.Name;
            ReportId = inspection.ReportId;
            StorageId = inspection.InspectedStorageId;
            DelegationIds = inspection.UserInspections.Select(ent => ent.UserId).ToList();
        }
    }
}