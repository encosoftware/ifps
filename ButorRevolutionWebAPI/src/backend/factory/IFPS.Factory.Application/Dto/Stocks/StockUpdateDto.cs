using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StockUpdateDto
    {
        public int PackageId { get; set; }
        public int StorageCellId { get; set; }
        public int Quantity { get; set; }

        public Stock UpdateModelObject(Stock stock)
        {
            stock.StorageCellId = StorageCellId;
            stock.PackageId = PackageId;
            stock.Quantity = Quantity;
            return stock;
        }
    }
}
