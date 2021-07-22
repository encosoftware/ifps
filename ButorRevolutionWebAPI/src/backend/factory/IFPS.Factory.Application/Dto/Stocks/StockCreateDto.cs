using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class StockCreateDto
    {
        public int PackageId { get; set; }
        public int StorageCellId { get; set; }
        public int Quantity { get; set; }

        public Stock CreateModelObject()
        {
            return new Stock(PackageId, Quantity, StorageCellId);
        }

        public Stock CreateModelObject(int packageId, int storageCellId, int quantity)
        {
            return new Stock(packageId, quantity, storageCellId);
        }
    }
}
