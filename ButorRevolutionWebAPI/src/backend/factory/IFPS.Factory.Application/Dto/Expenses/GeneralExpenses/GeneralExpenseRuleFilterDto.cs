using ENCO.DDD.Application.Dto;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseRuleFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int FrequencyFrom { get; set; }
        public int FrequencyTo { get; set; }
        public int FrequencyTypeId { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
            };
        }
    }
}
