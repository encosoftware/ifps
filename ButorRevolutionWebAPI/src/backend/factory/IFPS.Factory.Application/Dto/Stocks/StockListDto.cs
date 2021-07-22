using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class StockListDto
    {
        public int Id { get; set; }
        public string PackageDescription { get; set; }
        public string PackageCode { get; set; }
        public string StorageCellName { get; set; }
        public string StorageCellMetadata { get; set; }
        public int Quantity { get; set; }
        public string OrderName { get; set; }

        public StockListDto() { }

        public static Expression<Func<Stock, StockListDto>> Projection
        {
            get
            {
                return entity => new StockListDto
                {
                    Id = entity.Id,
                    PackageDescription = entity.Package.PackageDescription,
                    PackageCode = entity.Package.PackageCode,
                    StorageCellName = entity.StorageCell.Name,
                    StorageCellMetadata = entity.StorageCell.Metadata,
                    Quantity = entity.Quantity,
                    //OrderName = entity.OrderName
                };
            }
        }
    }
}
