using ENCO.DDD.Application.Dto;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class GeneralExpenseFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime? PaymentDateFrom { get; set; }
        public DateTime? PaymentDateTo { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
            };
        }       
    }
}
