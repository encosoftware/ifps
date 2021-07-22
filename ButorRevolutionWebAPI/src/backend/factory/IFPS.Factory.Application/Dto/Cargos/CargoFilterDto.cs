using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class CargoFilterDto : OrderedPagedRequestDto
    {
        public string CargoName { get; set; }

        public int? CargoStatusTypeId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? SupplierId { get; set; }

        public string BookedBy { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(CargoName), nameof(Cargo.CargoName) },
                { nameof(CargoStatusType), nameof(Cargo.Status) },
                { nameof(SupplierId), nameof(Cargo.Supplier.Name) },
                { nameof(BookedBy), nameof(Cargo.BookedBy.CurrentVersion.Name) },
            };
        }

    }
}
