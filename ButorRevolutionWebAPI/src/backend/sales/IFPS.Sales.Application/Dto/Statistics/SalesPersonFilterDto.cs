using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class SalesPersonFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(Order.SalesPerson.User.CurrentVersion.Name) }
            };
        }
    }
}
