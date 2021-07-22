using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StockDetailsDto
    {
        public int PackageId { get; set; }
        public int StorageCellId { get; set; }
        public int Quantity { get; set; }

        public StockDetailsDto(Stock stock)
        {
            PackageId = stock.PackageId;
            StorageCellId = stock.StorageCellId;
            Quantity = stock.Quantity;
        }
    }
}
