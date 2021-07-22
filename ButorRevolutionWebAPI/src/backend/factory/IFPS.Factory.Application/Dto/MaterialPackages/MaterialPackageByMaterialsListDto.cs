using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageByMaterialsListDto
    {
        public int Id { get; set; }
        public int PackageSize { get; set; }
        public string PackageName { get; set; }
        public PriceListDto Price { get; set; }

        public MaterialPackageByMaterialsListDto(MaterialPackage materialPackage)
        {
            Id = materialPackage.Id;
            PackageName = materialPackage.PackageCode;
            PackageSize = materialPackage.Size;
            Price = new PriceListDto(materialPackage.Price);
        }
    }
}
