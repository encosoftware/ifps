using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int PackageSize { get; set; }
        public int OrderedAmount { get; set; }
        public int Missing { get; set; }
        public int Refused { get; set; }

        public ProductListDto(OrderedMaterialPackage orderedPackage)
        {
            Id = orderedPackage.Id;
            MaterialCode = orderedPackage.MaterialPackage.Material.Code;
            MaterialName = orderedPackage.MaterialPackage.Material.Description;
            PackageCode = orderedPackage.MaterialPackage.PackageCode;
            PackageName = orderedPackage.MaterialPackage.PackageDescription;
            PackageSize = orderedPackage.MaterialPackage.Size;
            OrderedAmount = orderedPackage.OrderedAmount;
            Missing = orderedPackage.MissingAmount;
            Refused = orderedPackage.RefusedAmount;
        }
    }
}