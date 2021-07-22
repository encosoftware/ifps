using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class RequiredMaterialsFilterDto : OrderedPagedRequestDto
    {
        public string OrderName { get; set; }

        public string WorkingNumber { get; set; }

        public string MaterialCode { get; set; }

        public string Name { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(OrderName), nameof(RequiredMaterial.Order.OrderName) },
                { nameof(WorkingNumber), nameof(RequiredMaterial.Order.WorkingNumber) },
                { nameof(MaterialCode), nameof(RequiredMaterial.Material.Code) },
                { nameof(Name), nameof(RequiredMaterial.Material.Description) },
            };
        }
    }
}
