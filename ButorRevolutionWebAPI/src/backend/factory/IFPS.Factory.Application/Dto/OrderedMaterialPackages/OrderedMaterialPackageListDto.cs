using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class OrderedMaterialPackageListDto
    {
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int PackageSize { get; set; }
        public int Amount { get; set; }
        public PriceListDto PackagePrice { get; set; }
        public SubTotalPrice Subtotal { get; set; }

        public OrderedMaterialPackageListDto(OrderedMaterialPackage orderedPackage)
        {
            MaterialCode = orderedPackage.MaterialPackage.Material.Code;
            MaterialName = orderedPackage.MaterialPackage.Material.Description;
            PackageCode = orderedPackage.MaterialPackage.PackageCode;
            PackageName = orderedPackage.MaterialPackage.PackageDescription;
            PackageSize = orderedPackage.MaterialPackage.Size;
            Amount = orderedPackage.OrderedAmount;
            PackagePrice = new PriceListDto(orderedPackage.MaterialPackage.Price);
            Subtotal = new SubTotalPrice(orderedPackage.UnitPrice, orderedPackage.OrderedAmount);
        }
    }
}
