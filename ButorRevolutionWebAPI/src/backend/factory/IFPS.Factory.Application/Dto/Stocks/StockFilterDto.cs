using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class StockFilterDto : OrderedPagedRequestDto
    {
        public string PackageDescription { get; set; }
        public string PackageCode { get; set; }
        public string StorageCellName { get; set; }
        public string StorageCellMetadata { get; set; }
        public int Quantity { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
            };
        }

        public static Expression<Func<Stock, StockFilterDto>> GetProjection()
        {
            return x => new StockFilterDto
            {
                PackageDescription = x.Package.PackageDescription,
                PackageCode = x.Package.PackageCode,
                StorageCellName = x.StorageCell.Name,
                StorageCellMetadata = x.StorageCell.Metadata,
                Quantity = x.Quantity,
            };
        }
    }
}
