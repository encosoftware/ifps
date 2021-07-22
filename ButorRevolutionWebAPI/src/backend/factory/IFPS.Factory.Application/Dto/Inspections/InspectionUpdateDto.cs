using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionUpdateDto
    {
        public DateTime InspectedOn { get; set; }
        public string ReportName { get; set; }
        public int StorageId { get; set; }
        public List<int> DelegationIds { get; set; }

        public Inspection UpdateModelObject(Inspection inspection)
        {
            inspection.InspectedOn = InspectedOn;
            inspection.Report.Name = ReportName;
            inspection.InspectedStorageId = StorageId;
            inspection.UpdateUserInspections(DelegationIds);
            return inspection;
        }
    }
}
