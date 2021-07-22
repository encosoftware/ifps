using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class OrderedMaterialPackageCreateDto
    {
        public int PackageId { get; set; }
        public int OrderedPackageNum { get; set; }

        public OrderedMaterialPackage CreateModelObject(Price unitPrice)
        {
            return new OrderedMaterialPackage()
            {
                MaterialPackageId = PackageId,
                UnitPrice = new Price(unitPrice.Value, unitPrice.CurrencyId),
                OrderedAmount = this.OrderedPackageNum
            };
        }
    }
}
