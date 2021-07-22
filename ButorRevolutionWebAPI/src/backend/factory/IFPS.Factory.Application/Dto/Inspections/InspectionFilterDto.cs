using ENCO.DDD.Application.Dto;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionFilterDto : OrderedPagedRequestDto
    {
        public DateTime InspectedOn { get; set; }
        public int StorageId { get; set; }
        public string ReportName { get; set; }
        public string Delegation { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
            };
        }
    }
}
