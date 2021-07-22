using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionCreateDto
    {
        public DateTime InspectedOn { get; set; }
        public string ReportName { get; set; }
        public int StorageId { get; set; }
        public List<int> DelegationIds { get; set; }

        public InspectionCreateDto()
        {
        }

        public Inspection CreateModelObject()
        {
            return new Inspection(Clock.Now, StorageId);
        }
    }
}